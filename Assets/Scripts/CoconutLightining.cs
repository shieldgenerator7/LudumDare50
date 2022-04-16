using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutLightining : MonoBehaviour
{
	public GameObject lightningBolt;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnCollisionEnter2D(Collision2D other)
    {
		 Instantiate(lightningBolt, transform.position, Quaternion.identity);
		 Destroy(gameObject);
	}
}
