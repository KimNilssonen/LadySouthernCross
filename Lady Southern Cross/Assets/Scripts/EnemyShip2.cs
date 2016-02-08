using UnityEngine;
using System.Collections;

public class EnemyShip2 : EnemyShip1
{
	void Start()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2(Random.Range(-speed.x, speed.x), speed.y);
	}

	void FixedUpdate()
	{
		
		// Boundry of the enemy ship.
		GetComponent<Rigidbody2D> ().position = new Vector2
			(
				Mathf.Clamp(GetComponent<Rigidbody2D> ().position.x, boundary.xMin, boundary.xMax),
				Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax)
				);
		
		//Directions on second enemy ship.
		// Change direction to left.
		if (GetComponent<Rigidbody2D> ().position.x == boundary.xMax) 
		{
			GetComponent<Rigidbody2D> ().velocity = new Vector2(-speed.x, speed.y);
		}
		
		// Change direction to right.
		if (GetComponent<Rigidbody2D> ().position.x == boundary.xMin) 
		{
			GetComponent<Rigidbody2D> ().velocity = new Vector2(speed.x, speed.y);
		}

	}



}
