using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialScale : MonoBehaviour
{
	//the puprpose of this script is to scale collision sprers with graphics
	public float speed = 1f;

    void Update()
    {
        transform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0f);
    }
}
