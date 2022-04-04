using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitButton : MenuButton
{
    public float minYPos = -5;

    private List<MenuButton> buttons;
    private PlayerController playerController;
    private bool quitting = false;

    protected override void takeAction()
    {
        quitting = true;
        //Make all buttons fall
        buttons = FindObjectsOfType<MenuButton>().ToList();
        buttons.ForEach(btn =>
        {
            Rigidbody2D rb2d = btn.GetComponent<Rigidbody2D>();
            rb2d.gravityScale = 1;
            rb2d.constraints = RigidbodyConstraints2D.None;
            rb2d.drag = 0;
        });
        //Make player fall
        playerController = FindObjectOfType<PlayerController>();
        Rigidbody2D rb2d = playerController.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 1;
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.drag = 0;
        playerController.GetComponents<Collider2D>().ToList().ForEach(coll => coll.isTrigger = true);
    }

    private void Update()
    {
        if (quitting)
        {
            if (buttons.All(btn => btn.transform.position.y <= minYPos)
                && playerController.transform.position.y <= minYPos)
            {                
                quit();
            }
        }
    }

    private void quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
