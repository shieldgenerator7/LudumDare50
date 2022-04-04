using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditBoxButton : MenuButton
{
    public float showYPos = 0;
    public float moveSpeed = 10;

    private bool shown = false;
    private float targetY = 0;
    private bool moveActive = false;

    private float origY;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        origY = transform.position.y;
        targetY = origY;
        shown = false;
        updateShowState();
    }

    protected override void takeAction()
    {
        toggleShow(!shown);
    }

    public void toggleShow(bool show)
    {
        shown = show;
        updateShowState();
    }

    private void updateShowState()
    {
        if (shown)
        {
            targetY = showYPos;
        }
        else
        {
            targetY = origY;
        }
        moveActive = true;
    }

    private void Update()
    {
        if (moveActive)
        {
            float posY = transform.position.y;
            float diff = targetY - posY;
            rb2d.velocity = Vector2.up * Mathf.Sign(diff) * moveSpeed;
            if (shown && posY >= targetY || !shown && posY <= targetY)
            {
                rb2d.velocity = Vector2.zero;
                moveActive = false;
            }
        }
    }
}
