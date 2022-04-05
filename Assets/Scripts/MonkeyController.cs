using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    [Tooltip("When the monkey falls off, how high into the air he jumps first")]
    public float surpriseForce = 10;

    public GridAI gridAI;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //Animator
        animator = GetComponent<Animator>();
        updateAnimator();
    }

    private void OnDestroy()
    {
        Destroy(gridAI.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CoconutController>())
        {
            FallOff();
        }
    }

    private void Update()
    {
        updateAnimator();
    }

    private void updateAnimator()
    {
        //Animator
        Vector2 moveDir = gridAI.moveDir;
        animator.SetBool("climbVert", !Mathf.Approximately(moveDir.y, 0));
        animator.SetBool("climbHoriz", !Mathf.Approximately(moveDir.x, 0));
        //Transform
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDir.x);
        transform.localScale = scale;
    }

    private void FallOff()
    {
        Rigidbody2D rb2d = gameObject.AddComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.up * surpriseForce;
        rb2d.gravityScale = 1;
        this.enabled = false;
        GetComponents<Collider2D>().ToList().ForEach(coll => coll.isTrigger = true);
        updateAnimator();
    }
}
