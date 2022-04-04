using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public float holdDuration = 1;
    public float fadeDuration = 0.5f;

    private float showStartTime = 0;

    private SpriteRenderer sr;
    private TMP_Text txt;

    // Start is called before the first frame update
    void Start()
    {
        showStartTime = Time.time;
        //Renderers
        sr = GetComponentInChildren<SpriteRenderer>() ?? GetComponent<SpriteRenderer>();
        txt = GetComponentInChildren<TMP_Text>();
        Color c;
        //Sprite color
        c = sr.color;
        c.a = 1;
        sr.color = c;
        //Text color
        c = txt.color;
        c.a = 1;
        txt.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= showStartTime + holdDuration)
        {
            float alpha = 1 - ((Time.time - (showStartTime + holdDuration)) / fadeDuration);
            Color c;
            //Sprite color
            c = sr.color;
            c.a = alpha;
            sr.color = c;
            //Text color
            c = txt.color;
            c.a = alpha;
            txt.color = c;
            //Destroy
            if (alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
