using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private bool inTrees = false;
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

    //startV is my starting position on the grid
    Vector2Int startV;
    //endV is my ending position on the grid
    Vector2Int endV;

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
        path = AIGridManager.GetPath(startV, endV);
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
                startV = gridStats.v;
                path.RemoveAt(step);
                moveStep = moveStep - 1;
            }
        }
    }

    void SetTargetPosition()
    {
        if (startV.x == 5 && startV.y == 4)
        {
            HasBanana = true;
            if (exitRight)
            {
                endV = new Vector2Int(10, 0);
            }
            else
            {
                endV = new Vector2Int(0, 0);
            }
            findDistance = true;
        }
        else
        {
            endV = new Vector2Int(5, 4);
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
        startV.x = Mathf.CeilToInt(Random.Range(0f, 10f));
    }

    void StartTop()
    {
        startV.y = 4;
    }

    void SetTargetPositionTrees()
    {
        if (startV.x == 5 && startV.y == 4)
        {
            HasBanana = true;
            if (exitRight)
            {
                endV = new Vector2Int(10, 4);
            }
            else
            {
                endV = new Vector2Int(0, 4);
            }
            findDistance = true;
        }
        else
        {
            endV = new Vector2Int(5, 4);
        }
    }
}

