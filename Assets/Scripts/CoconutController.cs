using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutController : MonoBehaviour
{
    public bool countHitMonkey = true;
    public bool countHitPlayer = false;
    public bool countHitCoconut = false;

    private int hitCount = 0;
    public int HitCount
    {
        get => hitCount;
        private set
        {
            hitCount = value;
            onHitCountUpdated?.Invoke(hitCount);
        }
    }
    public delegate void OnHitCountUpdated(int hitCount);
    public event OnHitCountUpdated onHitCountUpdated;

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
        //Hit Counter
        if (countHitMonkey && collision.gameObject.GetComponent<MonkeyController>())
        {
            HitCount++;
        }
        else if (countHitPlayer && collision.gameObject.GetComponent<PlayerController>())
        {
            HitCount++;
        }
        else if (countHitCoconut && collision.gameObject.GetComponent<CoconutController>())
        {
            HitCount++;
        }
        else
        {
            onHitOther?.Invoke();
        }
    }
    public delegate void OnHitOther();
    public event OnHitOther onHitOther;
}
