using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShow : MonoBehaviour
{
    [Header("Settings")]
    public float maxSlideDuration = 5;

    [Header("Components")]
    public List<Sprite> slides;

    private bool playing = false;
    private int currentSlide = 0;
    public int SlideIndex
    {
        get => currentSlide;
        set
        {
            currentSlide = Mathf.Clamp(value, 0, slides.Count - 1);
            sr.sprite = slides[currentSlide];
            if (value >= slides.Count)
            {
                playing = false;
                onSlideSlowFinished?.Invoke();
            }
        }
    }
    public delegate void OnSlideSlowFinished();
    public event OnSlideSlowFinished onSlideSlowFinished;

    private float lastSlideStartTime = 0;

    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (Time.time >= lastSlideStartTime + maxSlideDuration)
            {
                nextSlide();
            }
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    nextSlide();
            //}
        }
    }

    public void startSlideShow()
    {
        playing = true;
        lastSlideStartTime = Time.time;
        SlideIndex = 0;
    }

    public void nextSlide()
    {
        if (playing)
        {
            lastSlideStartTime = Time.time;
            SlideIndex++;
        }
    }
}
