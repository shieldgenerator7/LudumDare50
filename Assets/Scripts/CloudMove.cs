using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
	public float speedMax = 1f;
	public float speedMin = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         transform.Translate(Vector3.right * Time.deltaTime * Random.Range(speedMin, speedMax));

    }
}
