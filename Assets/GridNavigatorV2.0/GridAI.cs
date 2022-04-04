﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAI : MonoBehaviour
{
    public Vector2 moveDir { get; private set; }

    public List<GameObject> path = new List<GameObject>();
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
    public GameObject[,] gridArray;

    //startX and startY are my starting position
    [SerializeField]
    int startX = 0;
    [SerializeField]
    int startY = 0;
    //endX and endY are my ending position on the grid
    [SerializeField]
    int endX = 0;
    [SerializeField]
    int endY = 0;
    [SerializeField]
    bool hasBanana = false;
    [SerializeField]
    bool exitRight = false;

    //find distance starts the process of moving
    public bool findDistance = true;

    //entrance/ exit ground left				10, 0
    //entrance/ exit ground right 				0, 0 
    //banana hoard 								5, 5
    //sneaky entrance left						0, 4
    //sneaky entrance right						10, 4			


    public Transform objectToMove;
    public float speed = 3;
    int moveStep = 0;

    // Start is called before the first frame update
    void Awake()
    {
        RandomStart();
        gridArray = new GameObject[columns, rows];
        if (gridPrefab)
        {

            GenerateGrid();
            gameObject.DeleteChildrenWithName(new[]{
                "grid01",
                "grid02",
                "grid03",
                "grid05",
                "grid11",
                "grid12",
                "grid13",
                "grid15",
                "grid25",
                "grid35",
                "grid45",
                "grid55",
                "grid65",
                "grid75",
                "grid85",
                "grid91",
                "grid92",
                "grid93",
                "grid95",
                "grid101",
                "grid102",
                "grid103",
                "grid105",
            });
        }
        else
        {
            print("Missing assigned gridPrefab");
        }

    }

    // Update is called once per frame
    void Update()
    {
        SetTargetPosition();
        SetSpeed();
        if (path.Count > 0)
            findDistance = false;
        if (findDistance)
        {

            SetDistance();
            SetPath();
        }

        if (startX == endX && startY == endY)
            ResetGridArrary();
        MoveIt(objectToMove);
    }
    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y + scale * j, leftBottomLocation.z), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStats>().scale = scale;
                obj.GetComponent<GridStats>().x = i;
                obj.GetComponent<GridStats>().y = j;
                obj.name = "grid" + i.ToString() + j.ToString();
                gridArray[i, j] = obj;
            }
        }
        ResetGridArrary();
    }

    void SetDistance()
    {
        InitialSetUp();
        int x = Mathf.RoundToInt(startX);
        int y = Mathf.RoundToInt(startY);
        int[] testArray = new int[rows * columns];

        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                if (obj)
                {
                    if (obj.GetComponent<GridStats>().visited == step - 1)
                    {
                        TestFourDirections(obj.GetComponent<GridStats>().x, obj.GetComponent<GridStats>().y, step);
                    }
                }
            }
        }

        moveStep = path.Count - 1;
        findDistance = false;

    }
    void InitialSetUp()
    {
        foreach (GameObject obj in gridArray)
        {
            if (obj)
                obj.GetComponent<GridStats>().visited = -1;
        }
        gridArray[Mathf.RoundToInt(startX), Mathf.RoundToInt(startY)].GetComponent<GridStats>().visited = 0;
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
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
            if (TestDirection(x, y, step, 1))
            {
                tempList.Add(gridArray[x, y + 1]);
                // y = y + 1;
            }
            if (TestDirection(x, y, step, 2))
            {
                tempList.Add(gridArray[x + 1, y]);
                // x = x + 1;
            }
            if (TestDirection(x, y, step, 3))
            {
                tempList.Add(gridArray[x, y - 1]);
                // y = y - 1;

            }
            if (TestDirection(x, y, step, 4))
            {
                tempList.Add(gridArray[x - 1, y]);
                //  x = x - 1;
            }
            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;
            tempList.Clear();

        }
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetVisited(x, y + 1, step);
        if (TestDirection(x, y, -1, 2))
            SetVisited(x + 1, y, step);
        if (TestDirection(x, y, -1, 3))
            SetVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4))
            SetVisited(x - 1, y, step);
    }


    void MoveIt(Transform obj)
    {
        int step = path.Count - 1;
        if (step > -1 && path.Count > 0 && step < path.Count)
        {
            moveDir = (path[step].transform.position - obj.position).normalized;
            obj.position = Vector3.MoveTowards(obj.position, path[step].transform.position, speed * Time.deltaTime);
            float dist = Vector3.Distance(obj.transform.position, path[step].transform.localPosition);
            if (dist < .05f)
            {
                startX = path[step].GetComponent<GridStats>().x;
                startY = path[step].GetComponent<GridStats>().y;
                path.RemoveAt(step);
                moveStep = moveStep - 1;

            }
        }
    }
    bool TestDirection(int x, int y, int step, int direction)
    {
        //int direction tells which case to use. 1 is up, 2, is to the right, 3 is bottom, 4 is to the left.
        switch (direction)
        {
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
        }
        return false;
    }
    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y] != null)
            gridArray[x, y].GetComponent<GridStats>().visited = step;
    }
    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
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
        foreach (GameObject obj in gridArray)
        {
            if (obj)
            {
                obj.GetComponent<GridStats>().visited = -1;

            }
        }
    }

    void SetTargetPosition()
    {
        if (startX == 5 && startY == 4)
        {
            hasBanana = true;
            if (exitRight)
            {
                endX = 10;
                endY = 0;
            }
            else
            {
                endX = 0;
                endY = 0;
            }

            findDistance = true;
        }

        else
        {
            endX = 5;
            endY = 4;
        }

    }

    void SetSpeed()
    {
        if (hasBanana)
        {
            speed = Random.Range(.1f, 2f);
        }
        else
        {
            speed = Random.Range(2f, 4f);
        }

    }

    void RandomStart()
    {
        startX = Mathf.CeilToInt(Random.Range(0f, 10f));
    }
}

