using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMover : MonoBehaviour
{
    public float showYPos = 2;
    public float hideYPos = -3;
    public float moveSpeed = 10;

    private bool shown = false;
    private float targetY = 0;
    private bool moveActive = false;

    public List<ButtonMover> hideFollowers;

    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //shown = false;
        //updateShowState();
    }

    public void toggleShow()
    {
        toggleShow(!shown);
    }

    public void toggleShow(bool show)
    {
        shown = show;
        updateShowState();
        if (!show)
        {
            hideFollowers.ForEach(mvr => mvr.toggleShow(show));
        }
    }

    private void updateShowState()
    {
        if (shown)
        {
            targetY = showYPos;
        }
        else
        {
            targetY = hideYPos;
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
