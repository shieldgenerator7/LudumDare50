using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDisabler : MonoBehaviour
{
	public float waitTime 	= 5.0f;
    private float timer 	= 0.0f;


	void Start (){	gameObject.SetActive(false);}
    void Update()
    {
		if (gameObject.activeSelf){
			timer += Time.deltaTime;

			if (timer > waitTime)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
