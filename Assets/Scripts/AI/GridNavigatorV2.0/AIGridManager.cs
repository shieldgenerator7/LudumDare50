using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIGridManager : MonoBehaviour
{
    [SerializeField]
    private int rows = 10;
    [SerializeField]
    private int columns = 10;
    [SerializeField]
    private float scale = 1.7f;
    [SerializeField]
    private GameObject gridPrefab;
    [SerializeField]
    private Vector3 leftBottomLocation = new Vector3(0, 0, 0);

    private Dictionary<Vector2Int, GridStats> gridArray;


    private static List<Vector2Int> directions = new List<Vector2Int>()
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

    //entrance/ exit ground left				10, 0
    //entrance/ exit ground right 				0, 0 
    //banana hoard 								5, 5
    //sneaky entrance left						0, 4
    //sneaky entrance right						10, 4			
    private static AIGridManager instance;
    private void Awake()
    {
        instance ??= this;
        populateGridArray();
    }
    public void GenerateGridObjects()
    {
        if (!gridPrefab)
        {
            Debug.LogError($"Missing assigned gridPrefab! gridPrefab: {gridPrefab}");
        }
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(
                    gridPrefab,
                    new Vector3(
                        leftBottomLocation.x + scale * i,
                        leftBottomLocation.y + scale * j,
                        leftBottomLocation.z
                        ),
                    Quaternion.identity
                    );
                obj.transform.SetParent(gameObject.transform);
                GridStats gridStats = obj.GetComponent<GridStats>();
                gridStats.scale = scale;
                gridStats.v = new Vector2Int(i, j);
                obj.name = "grid" + i.ToString() + j.ToString();
            }
        }
    }
    void populateGridArray()
    {
        gridArray = new Dictionary<Vector2Int, GridStats>();
        List<GridStats> gridStatsList = gameObject.GetComponentsInChildren<GridStats>().ToList();
        gridStatsList.ForEach(gridStats =>
        {
            gridArray[gridStats.v] = gridStats;
        });
    }

    Dictionary<Vector2Int, VisitData> getFreshVisitDataArray()
    {
        Dictionary<Vector2Int, VisitData> visitArray = new Dictionary<Vector2Int, VisitData>();
        foreach (GridStats gridStats in gridArray.Values)
        {
            if (gridStats)
            {
                VisitData visit = new VisitData();
                visit.visited = -1;
                visitArray[gridStats.v] = visit;
            }
        }
        return visitArray;
    }

    void SetDistance(Dictionary<Vector2Int, VisitData> visitData)
    {
        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GridStats gridStats in gridArray.Values)
            {
                if (gridStats)
                {
                    if (visitData[gridStats.v].visited == step - 1)
                    {
                        TestFourDirections(gridStats.v, step, visitData);
                    }
                }
            }
        }
    }
    void InitialSetUp(Vector2Int startV, Dictionary<Vector2Int, VisitData> visitData)
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                SetVisited(new Vector2Int(i, j), -1, visitData);
            }
        }
        SetVisited(startV, 0, visitData);
    }

    public static List<GridStats> GetPath(Vector2Int startV, Vector2Int endV)
    {
        return instance._GetPath(startV, endV);
    }
    private List<GridStats> _GetPath(Vector2Int startV, Vector2Int endV)
    {
        if (!gridArray.ContainsKey(endV) || (startV.x == endV.x && startV.y == endV.y))
        {
            Debug.LogWarning("Can't reach the desired location or you are currently at the position.");
            return new List<GridStats>();
        }

        List<GridStats> path = new List<GridStats>();
        List<GridStats> optionsList = new List<GridStats>();
        Dictionary<Vector2Int, VisitData> visitData = getFreshVisitDataArray();
        InitialSetUp(startV, visitData);
        SetDistance(visitData);

        Vector2Int v = endV;
        int step = GetVisit(gridArray[v], visitData) - 1;
        path.Add(gridArray[v]);
        for (; step > -1; step--)
        {
            directions
                .FindAll(dir => TestDirection(v, step, dir, visitData))
                .ForEach(dir => optionsList.Add(gridArray[v + dir]));
            GridStats tempObj = FindClosest(gridArray[endV].transform, optionsList);
            path.Add(tempObj);
            v = tempObj.v;
            optionsList.Clear();
        }
        return path;
    }

    void TestFourDirections(Vector2Int v, int step, Dictionary<Vector2Int, VisitData> visitData)
    {
        directions
            .FindAll(dir => TestDirection(v, -1, dir, visitData))
            .ForEach(dir => SetVisited(v + dir, step, visitData));
    }

    bool TestDirection(Vector2Int v, int step, Vector2Int direction, Dictionary<Vector2Int, VisitData> visitData)
    {
        return HasBeenVisited(v + direction, step, visitData);
    }
    bool HasBeenVisited(Vector2Int v, int step, Dictionary<Vector2Int, VisitData> visitData)
    {
        return v.x >= 0 && v.x < columns
            && v.y >= 0 && v.y < rows
            && gridArray[v]
            && GetVisit(gridArray[v], visitData) == step
            ;
    }
    void SetVisited(Vector2Int v, int step, Dictionary<Vector2Int, VisitData> visitData)
    {
        GridStats gridStats = gridArray[v];
        if (gridStats)
        {
            VisitData vida = visitData[gridStats.v];
            vida.visited = step;
            visitData[gridStats.v] = vida;
        }
    }
    int GetVisit(GridStats gridStats, Dictionary<Vector2Int, VisitData> visitData)
    {
        if (!gridStats)
        {
            return -1;
        }
        return visitData[gridStats.v].visited;
    }
    VisitData GetVisitData(GridStats gridStats, Dictionary<Vector2Int, VisitData> visitData)
    {
        if (!gridStats)
        {
            throw new System.ArgumentException($"Can't return any VisitData! gridStats: {gridStats}");
        }
        return visitData[gridStats.v];
    }
    GridStats FindClosest(Transform targetLocation, List<GridStats> list)
    {
        float currentDistance = scale * rows + scale * 2 * columns;
        int indexNumber = 0;

        for (int i = 0; i < list.Count; i++)
        {
            float newDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
            if (newDistance < currentDistance)
            {
                currentDistance = newDistance;
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }

}

