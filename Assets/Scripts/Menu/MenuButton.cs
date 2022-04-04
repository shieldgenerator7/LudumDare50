using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuButton : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CoconutController coconut = collision.gameObject.GetComponent<CoconutController>();
        if (coconut)
        {
            takeAction();
        }
    }

    protected abstract void takeAction();
}
