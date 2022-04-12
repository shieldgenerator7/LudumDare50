using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private  bool inTrees = false;
    [SerializeField]
    private bool exitRight = false;
    [SerializeField]
    private float minSpeed = 2.5f;
    [SerializeField]
    private float maxSpeed = 4.0f;
    [SerializeField]
    private float minSpeedBanana = 0.6f;
    [SerializeField]
    private float maxSpeedBanana = 2.0f;

    [Header("Components")]
    public Transform objectToMove;


    private List<GridStats> path = new List<GridStats>();

    //startX and startY are my starting position
    int startX = 0;
    int startY = 0;
    //endX and endY are my ending position on the grid
    int endX = 0;
    int endY = 0;

    private float speed = 3;
    private int moveStep = 0;
    public Vector2 moveDir { get; private set; }

    bool hasBanana = false;
    public bool HasBanana
    {
        get => hasBanana;
        set
        {
            hasBanana = value;
            onHasBananaChanged?.Invoke(hasBanana);
        }
    }
    public delegate void OnHasBananaChanged(bool has);
    public event OnHasBananaChanged onHasBananaChanged;

    //find distance starts the process of moving
    bool findDistance = true;

    //entrance/ exit ground left				10, 0
    //entrance/ exit ground right 				0, 0
    //banana hoard 								5, 5
    //sneaky entrance left						0, 4
    //sneaky entrance right						10, 4

    void Awake()
    {
        if (inTrees)
        {
            StartTop();
        }
        RandomStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrees)
        {
            SetTargetPositionTrees();
            SetSpeed();
            if (path.Count > 0)
            {
                findDistance = false;
            }
            if (findDistance)
            {
                SetDistance();
                SetPath();
            }
            MoveIt(objectToMove);
        }
        else
        {
            SetTargetPosition();
            SetSpeed();
            if (path.Count > 0)
            {
                findDistance = false;
            }
            if (findDistance)
            {
                SetDistance();
                SetPath();
            }
            MoveIt(objectToMove);
        }

    }


    void SetDistance()
    {
        moveStep = path.Count - 1;
        findDistance = false;
    }

    void SetPath()
    {
        path.Clear();
        path = AIGridManager.GetPath(startX, startY, endX, endY);
    }



    void MoveIt(Transform obj)
    {
        int step = path.Count - 1;
        if (step > -1 && path.Count > 0 && step < path.Count)
        {
            GridStats gridStats = path[step];
            moveDir = (gridStats.transform.position - obj.position).normalized;
            obj.position = Vector3.MoveTowards(
                obj.position,
                gridStats.transform.position,
                speed * Time.deltaTime
                );
            float dist = Vector3.Distance(
                obj.transform.position,
                gridStats.transform.localPosition
                );
            if (dist < .05f)
            {
                startX = gridStats.x;
                startY = gridStats.y;
                path.RemoveAt(step);
                moveStep = moveStep - 1;
            }
        }
    }

    void SetTargetPosition()
    {
        if (startX == 5 && startY == 4)
        {
            HasBanana = true;
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
            speed = Random.Range(minSpeedBanana, maxSpeedBanana);
        }
        else
        {
            speed = Random.Range(minSpeed, maxSpeed);
        }
    }

    void RandomStart()
    {
        startX = Mathf.CeilToInt(Random.Range(0f, 10f));
    }

    void StartTop()
    {
        startY = 4;
    }

    void SetTargetPositionTrees()
    {
        if (startX == 5 && startY == 4)
        {
            hasBanana = true;
            if (exitRight)
            {
                endX = 10;
                endY = 4;
            }
            else
            {
                endX = 0;
                endY = 4;
            }
            findDistance = true;
        }
        else
        {
            endX = 5;
            endY = 4;
        }
    }
}

