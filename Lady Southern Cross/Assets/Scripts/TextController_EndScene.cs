using UnityEngine;
using System.Collections;

public class TextController_EndScene : MonoBehaviour 
{

	Renderer rend;

	void Start()
	{

		rend = GetComponent<Renderer> ();
		Time.timeScale = 1;
	
	}
	
	void OnMouseEnter()
	{
		// Change color of the text.
		rend.material.color = Color.black;
	}
	
	void OnMouseExit() 
	{
		// Change color of the text.
		rend.material.color = Color.white;
	}
	
	void OnMouseUpAsButton()
	{
		if (gameObject.tag == "Restart") 
		{
			Destroy(GameObject.FindGameObjectWithTag("Music"));
			// Load game.
			Application.LoadLevel ("Main");
		} 
		else 
		{
			Application.LoadLevel("Start");
		}

	}
}
