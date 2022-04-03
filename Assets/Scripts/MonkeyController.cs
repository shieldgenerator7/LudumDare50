using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    public float moveSpeed = 3;

    private Vector2 moveDir = Vector2.up;

    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = moveDir * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Destroy(this);
            GetComponents<Collider2D>().ToList().ForEach(coll => coll.isTrigger = true);
        }
    }
}
