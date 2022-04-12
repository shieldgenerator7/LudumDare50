using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CartoonCoffee
{
    public class DemoTarget : MonoBehaviour
    {
        SpriteRenderer sr;

        Color originalColor;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();

            originalColor = sr.color;
        }

        public void Impact()
        {
            sr.color = Color.white;
            transform.localScale = new Vector3(0.65f, 0.65f, 1f);
        }

        void Update()
        {
            sr.color = Color.Lerp(sr.color, originalColor, Time.deltaTime * 5f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 8f);
        }
    }
}
