using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 3;
    public int moveVotesUp = 10;
    public int maxMoveVotesDown = 5;
    public int maxMoveVotesSideways = 8;
    [Tooltip("When the monkey falls off, how high into the air he jumps first")]
    public float surpriseForce = 10;
    [Tooltip("How close a vine must be to it in order to snap to it")]
    public float branchRangeRequirement = 0.2f;
    [Header("Components")]
    public Transform graspPoint;

    private Vector2 moveDir = Vector2.up;
    private List<Vector2> movePossibilities;

    private Branch currentBranch;

    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        //RB2D
        rb2d = GetComponent<Rigidbody2D>();
        //Move possibilities
        movePossibilities = new List<Vector2>();
        for (int i = 0; i < moveVotesUp; i++)
        {
            movePossibilities.Add(Vector2.up);
        }
        int moveVotesDown = Random.Range(0, maxMoveVotesDown + 1);
        for (int i = 0; i < moveVotesDown; i++)
        {
            movePossibilities.Add(Vector2.down);
        }
        int moveVotesSideways = Random.Range(0, maxMoveVotesSideways + 1);
        for (int i = 0; i < moveVotesSideways; i++)
        {
            movePossibilities.Add(Vector2.left);
            movePossibilities.Add(Vector2.right);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBranch)
        {
            rb2d.velocity = moveDir * moveSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Branch branch = collision.gameObject.GetComponent<Branch>();
        if (branch)
        {
            checkChangeDirection(branch);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Branch branch = collision.gameObject.GetComponent<Branch>();
        if (branch)
        {
            checkChangeDirection(branch);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Branch branch = collision.gameObject.GetComponent<Branch>();
        if (branch)
        {
            if (branch == currentBranch)
            {
                //fall
                rb2d.gravityScale = 1;
                currentBranch = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CoconutController>())
        {
            FallOff();
        }
    }

    private void FallOff()
    {
        rb2d.velocity = Vector2.up * surpriseForce;
        rb2d.gravityScale = 1;
        Destroy(this);
        GetComponents<Collider2D>().ToList().ForEach(coll => coll.isTrigger = true);
    }

    private void checkChangeDirection(Branch branch)
    {
        //If the given branch is in range
        Vector2 closestPoint = branch.GetComponent<Collider2D>().ClosestPoint(graspPoint.position);
        if (Vector2.Distance(closestPoint, graspPoint.position) <= branchRangeRequirement)
        {
            if (!currentBranch)
            {
                //do nothing
            }
            else
            {
                //decide which direction to go
                changeDirection();
            }
            //Snap onto the branch
            graspBranchIfNeeded(branch);
        }
    }

    private void changeDirection()
    {
        int rand = Random.Range(0, movePossibilities.Count);
        moveDir = movePossibilities[rand];
    }

    private void graspBranchIfNeeded(Branch branch)
    {
        //Check to see if branch should be switched
        if (!currentBranch)
        {
            currentBranch = branch;
            moveInBranchDirectionIfNeeded(currentBranch);
        }
        else if (!currentBranch.canTraverseDirection(moveDir))
        {
            currentBranch = branch;
        }
        //Check to make sure you can traverse in your direction on the branch
        if (currentBranch.canTraverseDirection(moveDir))
        {
            graspBranch(currentBranch);
            rb2d.gravityScale = 0;
        }
        else
        {
            currentBranch = null;
            rb2d.gravityScale = 1;
        }
    }

    void moveInBranchDirectionIfNeeded(Branch branch)
    {
        if (!movePossibilities.Any(v => branch.canTraverseDirection(v)))
        {
            movePossibilities.Add(Vector2.up);
            movePossibilities.Add(Vector2.down);
            movePossibilities.Add(Vector2.left);
            movePossibilities.Add(Vector2.right);
        }
        while (!branch.canTraverseDirection(moveDir))
        {
            changeDirection();
        }
    }

    void graspBranch(Branch branch)
    {
        Vector2 offset = -graspPoint.transform.localPosition;
        Vector2 position = transform.position;
        switch (branch.orientation)
        {
            case Branch.Option.HORIZONTAL:
                position.y = branch.transform.position.y + offset.y;
                break;
            case Branch.Option.VERTICAL:
                position.x = branch.transform.position.x + offset.x;
                break;
            default:
                throw new System.NotImplementedException($"Unknown option: {branch.orientation}");
        }
        transform.position = position;
        rb2d.gravityScale = 0;
    }
}
