using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocity = rb2d.velocity;
        velocity.x *= -1;
        velocity.y = Mathf.Abs(velocity.y);
        rb2d.velocity = velocity;
    }
}
