using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShow : MonoBehaviour
{
    [Header("Settings")]
    public float maxSlideDuration = 5;

    [Header("Components")]
    public List<Sprite> slides;

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
                onSlideSlowFinished?.Invoke();
            }
        }
    }
    public delegate void OnSlideSlowFinished();
    public event OnSlideSlowFinished onSlideSlowFinished;

    private float lastSlideStartTime = 0;

    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastSlideStartTime + maxSlideDuration)
        {
            nextSlide();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            nextSlide();
        }
    }

    public void startSlideShow()
    {
        lastSlideStartTime = Time.time;
        SlideIndex = 0;
    }

    public void nextSlide()
    {
        lastSlideStartTime = Time.time;
        SlideIndex++;
    }
}
