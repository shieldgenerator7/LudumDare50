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

    private GridStats[,] gridArray;

    public enum Direction
    {
        UP = 1,
        RIGHT = 2,
        DOWN = 3,
        LEFT = 4,
    }

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
                gridStats.x = i;
                gridStats.y = j;
                obj.name = "grid" + i.ToString() + j.ToString();
            }
        }
    }
    void populateGridArray()
    {
        gridArray = new GridStats[columns, rows];
        List<GridStats> gridStatsList = gameObject.GetComponentsInChildren<GridStats>().ToList();
        gridStatsList.ForEach(gridStats =>
        {
            gridArray[gridStats.x, gridStats.y] = gridStats;
        });
    }

    VisitData[,] getFreshVisitDataArray()
    {
        VisitData[,] visitArray = new VisitData[columns, rows];
        foreach (GridStats gridStats in gridArray)
        {
            if (gridStats)
            {
                VisitData visit = new VisitData();
                visit.visited = -1;
                visitArray[gridStats.x, gridStats.y] = visit;
            }
        }
        return visitArray;
    }

    void SetDistance(int startX, int startY, VisitData[,] visitData)
    {
        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GridStats gridStats in gridArray)
            {
                if (gridStats)
                {
                    if (visitData[gridStats.x, gridStats.y].visited == step - 1)
                    {
                        TestFourDirections(gridStats.x, gridStats.y, step, visitData);
                    }
                }
            }
        }
    }
    void InitialSetUp(int startX, int startY, VisitData[,] visitData)
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                SetVisited(i, j, -1, visitData);
            }
        }
        SetVisited(startX, startY, 0, visitData);
    }

    public static List<GridStats> GetPath(int startX, int startY, int endX, int endY)
    {
        return instance._GetPath(startX, startY, endX, endY);
    }
    private List<GridStats> _GetPath(int startX, int startY, int endX, int endY)
    {
        if (!gridArray[endX, endY] || (startX == endX && startY == endY))
        {
            Debug.LogWarning("Can't reach the desired location or you are currently at the position.");
            return new List<GridStats>();
        }

        List<GridStats> path = new List<GridStats>();
        List<GridStats> optionsList = new List<GridStats>();
        VisitData[,] visitData = getFreshVisitDataArray();
        InitialSetUp(startX, startY, visitData);
        SetDistance(startX, startY, visitData);

        int x = endX;
        int y = endY;
        int step = GetVisit(gridArray[x, y], visitData) - 1;
        path.Add(gridArray[x, y]);

        for (; step > -1; step--)
        {
            if (TestDirection(x, y, step, Direction.UP, visitData))
            {
                optionsList.Add(gridArray[x, y + 1]);
            }
            if (TestDirection(x, y, step, Direction.RIGHT, visitData))
            {
                optionsList.Add(gridArray[x + 1, y]);
            }
            if (TestDirection(x, y, step, Direction.DOWN, visitData))
            {
                optionsList.Add(gridArray[x, y - 1]);
            }
            if (TestDirection(x, y, step, Direction.LEFT, visitData))
            {
                optionsList.Add(gridArray[x - 1, y]);
            }
            GridStats tempObj = FindClosest(gridArray[endX, endY].transform, optionsList);
            path.Add(tempObj);
            x = tempObj.x;
            y = tempObj.y;
            optionsList.Clear();
        }
        return path;
    }

    void TestFourDirections(int x, int y, int step, VisitData[,] visitData)
    {
        if (TestDirection(x, y, -1, Direction.UP, visitData))
        {
            SetVisited(x, y + 1, step, visitData);
        }
        if (TestDirection(x, y, -1, Direction.RIGHT, visitData))
        {
            SetVisited(x + 1, y, step, visitData);
        }
        if (TestDirection(x, y, -1, Direction.DOWN, visitData))
        {
            SetVisited(x, y - 1, step, visitData);
        }
        if (TestDirection(x, y, -1, Direction.LEFT, visitData))
        {
            SetVisited(x - 1, y, step, visitData);
        }
    }

    bool TestDirection(int x, int y, int step, Direction direction, VisitData[,] visitData)
    {
        //int direction tells which case to use. 1 is up, 2, is to the right, 3 is bottom, 4 is to the left.
        switch (direction)
        {
            case Direction.UP:
                return HasBeenVisited(x, y + 1, step, visitData);
            case Direction.RIGHT:
                return HasBeenVisited(x + 1, y, step, visitData);
            case Direction.DOWN:
                return HasBeenVisited(x, y - 1, step, visitData);
            case Direction.LEFT:
                return HasBeenVisited(x - 1, y, step, visitData);
            default:
                throw new System.NotImplementedException($"Direction not recognized: {direction}");
        }
    }
    bool HasBeenVisited(int x, int y, int step, VisitData[,] visitData)
    {
        return x >= 0 && x < columns
            && y >= 0 && y < rows
            && gridArray[x, y]
            && GetVisit(gridArray[x, y], visitData) == step
            ;
    }
    void SetVisited(int x, int y, int step, VisitData[,] visitData)
    {
        GridStats gridStats = gridArray[x, y];
        if (gridStats)
        {
            visitData[gridStats.x, gridStats.y].visited = step;
        }
    }
    int GetVisit(GridStats gridStats, VisitData[,] visitData)
    {
        if (!gridStats)
        {
            return -1;
        }
        return visitData[gridStats.x, gridStats.y].visited;
    }
    VisitData GetVisitData(GridStats gridStats, VisitData[,] visitData)
    {
        if (!gridStats)
        {
            throw new System.ArgumentException($"Can't return any VisitData! gridStats: {gridStats}");
        }
        return visitData[gridStats.x, gridStats.y];
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

