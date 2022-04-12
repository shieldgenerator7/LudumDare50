using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIGridManager : MonoBehaviour
{
    [SerializeField]
    int rows = 10;
    [SerializeField]
    int columns = 10;
    [SerializeField]
    float scale = 1.7f;
    [SerializeField]
    GameObject gridPrefab;
    [SerializeField]
    Vector3 leftBottomLocation = new Vector3(0, 0, 0);
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
    private void Awake()
    {
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
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y + scale * j, leftBottomLocation.z), Quaternion.identity);
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
        ResetGridArrary();
    }

    void SetDistance(int startX, int startY)
    {
        InitialSetUp(startX, startY);
        int x = Mathf.RoundToInt(startX);
        int y = Mathf.RoundToInt(startY);
        int[] testArray = new int[rows * columns];

        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GridStats gridStats in gridArray)
            {
                if (gridStats)
                {
                    if (gridStats.visited == step - 1)
                    {
                        TestFourDirections(gridStats.x, gridStats.y, step);
                    }
                }
            }
        }
    }
    void InitialSetUp(int startX, int startY)
    {
        foreach (GridStats gridStats in gridArray)
        {
            if (gridStats)
                gridStats.visited = -1;
        }
        gridArray[Mathf.RoundToInt(startX), Mathf.RoundToInt(startY)].visited = 0;
    }

    void SetPath(int endX, int endY)
    {
        int step;
        int x = endX;
        int y = endY;
        List<GridStats> tempList = new List<GridStats>();
        List<GridStats> path = new List<GridStats>();
        path.Clear();
        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStats>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStats>().visited - 1;
        }
        else
        {

            print("Can't reach the desired location or you are currently at the position.");
            return;
        }
        for (int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, Direction.UP))
            {
                tempList.Add(gridArray[x, y + 1]);
                // y = y + 1;
            }
            if (TestDirection(x, y, step, Direction.RIGHT))
            {
                tempList.Add(gridArray[x + 1, y]);
                // x = x + 1;
            }
            if (TestDirection(x, y, step, Direction.DOWN))
            {
                tempList.Add(gridArray[x, y - 1]);
                // y = y - 1;

            }
            if (TestDirection(x, y, step, Direction.LEFT))
            {
                tempList.Add(gridArray[x - 1, y]);
                //  x = x - 1;
            }
            GridStats tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;
            tempList.Clear();

        }
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, Direction.UP))
        {
            SetVisited(x, y + 1, step);
        }
        if (TestDirection(x, y, -1, Direction.RIGHT))
        {
            SetVisited(x + 1, y, step);
        }
        if (TestDirection(x, y, -1, Direction.DOWN))
        {
            SetVisited(x, y - 1, step);
        }
        if (TestDirection(x, y, -1, Direction.LEFT))
        {
            SetVisited(x - 1, y, step);
        }
    }

    bool TestDirection(int x, int y, int step, Direction direction)
    {
        //int direction tells which case to use. 1 is up, 2, is to the right, 3 is bottom, 4 is to the left.
        switch (direction)
        {
            case Direction.UP:
                return (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].visited == step);
            case Direction.RIGHT:
                return (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].visited == step);
            case Direction.DOWN:
                return (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].visited == step);
            case Direction.LEFT:
                return (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].visited == step);
            default:
                throw new System.NotImplementedException($"Direction not recognized: {direction}");
        }
    }
    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y] != null)
        {
            gridArray[x, y].visited = step;
        }
    }
    GridStats FindClosest(Transform targetLocation, List<GridStats> list)
    {
        float currentDistance = scale * rows + scale * 2 * columns;
        int indexNumber = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);

                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
    void ResetGridArrary()
    {
        foreach (GridStats gridStats in gridArray)
        {
            if (gridStats)
            {
                gridStats.visited = -1;
            }
        }
    }

}

