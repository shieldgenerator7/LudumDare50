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
    [Header("Components")]
    public Transform graspPoint;

    private Vector2 moveDir = Vector2.up;
    private List<Vector2> movePossibilities;

    private Branch currentBranch;

    private Animator animator;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        //Animator
        animator = GetComponent<Animator>();
        updateAnimator();
        //RB2D
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
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
        rb2d.velocity = moveDir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Branch branch = collision.gameObject.GetComponent<Branch>();
        if (branch)
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

    private void updateAnimator()
    {
        //Animator
        animator.SetBool("climbVert", moveDir.x == 0 && moveDir.y != 0);
        animator.SetBool("climbHoriz", moveDir.x != 0 && moveDir.y == 0);
        //Transform
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDir.x);
        transform.localScale = scale;
    }

    private void FallOff()
    {
        rb2d.velocity = Vector2.up * surpriseForce;
        rb2d.gravityScale = 1;
        Destroy(this);
        GetComponents<Collider2D>().ToList().ForEach(coll => coll.isTrigger = true);
    }

    private void changeDirection()
    {
        int rand = Random.Range(0, movePossibilities.Count);
        moveDir = movePossibilities[rand];
        updateAnimator();
    }

    private void graspBranchIfNeeded(Branch branch)
    {
        if (!currentBranch)
        {
            currentBranch = branch;
        }
        else
        {
            if (currentBranch.canTraverseDirection(moveDir))
            {
                graspBranch(currentBranch);
            }
            else if (branch.canTraverseDirection(moveDir))
            {
                graspBranch(branch);
            }
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
