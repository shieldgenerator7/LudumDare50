using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class slideshow1 : MonoBehaviour
{
	
	public Texture[] imageArray; 
	private int currentImage;
		
	float deltaTime = 0.0f;

	public float timer1 = 5.0f;
    public float timer1Remaining = 5.0f;
    public bool timer1IsRunning = true;
  	public string timer1Text;
	public string sceneToLoad;
	
	void OnGUI()
	{
		
		int w = Screen.width, h = Screen.height;
		
		Rect imageRect = new Rect(0, 0, Screen.width, Screen.height);
		
 		//GUI.Label(imageRect, imageArray[currentImage]);
		//Draw texture seems more elegant
		GUI.DrawTexture(imageRect, imageArray[currentImage]);
	
		//if(GUI.Button(buttonRect, "Next"))
		//currentImage++;
		
		if(currentImage >= imageArray.Length)
			currentImage = 0;
	}
 
    // Start is called before the first frame update
    void Start()
    {
        currentImage = 0;
		bool timer1IsRunning = true;
		timer1Remaining = timer1;
	 }

    // Update is called once per frame
    void Update()
    {
		Cursor.visible= false;
		Screen.lockCursor = true;
				
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		
			
			if (Input.GetMouseButtonDown(0))
			{
				UnityEngine.Debug.Log("Pressed primary button.");
					currentImage++;
		
				if(currentImage >= imageArray.Length)
				{
					currentImage = 0;
					timer1Remaining = timer1;
					SceneManager.LoadScene(sceneToLoad);
				}
			}
			
			 if (timer1IsRunning)
			 
			{
				if (timer1Remaining > 0)
				{
					timer1Remaining -= Time.deltaTime;
					
				}
			
				else
				{
					UnityEngine.Debug.Log("Time has run out!");
				
					currentImage++;
		
					if(currentImage >= imageArray.Length)
					{
						currentImage = 0;
						timer1Remaining = timer1;
						SceneManager.LoadScene(sceneToLoad);
					}
			    }
			}
        
    }
}
