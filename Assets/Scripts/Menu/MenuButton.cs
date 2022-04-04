using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuButton : MonoBehaviour
{
    public float coconutLaunchForce = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CoconutController coconut = collision.gameObject.GetComponent<CoconutController>();
        if (coconut)
        {
            //Launch coconut
            if (coconutLaunchForce > 0)
            {
                coconut.GetComponent<Rigidbody2D>().velocity = Vector2.up * coconutLaunchForce;
            }
            //Take button action
            takeAction();
        }
    }

    protected abstract void takeAction();
}
