using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    public float moveSpeed = 3;
    public int moveVotesUp = 10;
    public int maxMoveVotesDown = 5;
    public int maxMoveVotesSideways = 8;
    [Tooltip("When the monkey falls off, how high into the air he jumps first")]
    public float surpriseForce = 10;

    private Vector2 moveDir = Vector2.up;
    private List<Vector2> movePossibilities;

    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
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
        if (collision.gameObject.GetComponent<Branch>())
        {
            changeDirection();
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

    private void changeDirection()
    {
        int rand = Random.Range(0, movePossibilities.Count);
        moveDir = movePossibilities[rand];
    }
}
