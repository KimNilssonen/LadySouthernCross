using UnityEngine;
using System.Collections;

public class TextControll_StartScene : MonoBehaviour 
{

	Renderer rend;
	public Canvas InfoMenu;

	void Start()
	{
		Destroy(GameObject.FindGameObjectWithTag("Music"));
		rend = GetComponent<Renderer> ();
		InfoMenu.enabled = false;
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
		if (gameObject.tag == "StartGame") 
		{
			// Load level
			Application.LoadLevel ("Main");
		}
		if (gameObject.tag == "Info") 
		{
			GameObject.FindGameObjectWithTag("StartGame").GetComponent<BoxCollider>().enabled = false;
			GameObject.FindGameObjectWithTag("Info").GetComponent<BoxCollider>().enabled = false;
			InfoMenu.enabled = true;

		}
	}

	public void CloseInfo()
	{
		InfoMenu.enabled = false;
		GameObject.FindGameObjectWithTag("StartGame").GetComponent<BoxCollider>().enabled = true;
		GameObject.FindGameObjectWithTag("Info").GetComponent<BoxCollider>().enabled = true;
	}
}
