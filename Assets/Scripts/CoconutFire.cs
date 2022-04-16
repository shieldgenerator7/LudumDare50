using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutFire : MonoBehaviour
{
  	public GameObject fireExplosion;
	
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
		 Instantiate(fireExplosion, transform.position, Quaternion.identity);
	}
}
