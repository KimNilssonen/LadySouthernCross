using UnityEngine;
using System.Collections;

public class EnemyShip3 : EnemyShip1
{
	Rigidbody2D rigid;

	// Use this for initialization
	void Start () 
	{
		rigid = GetComponent<Rigidbody2D> ();
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
		float x = rigid.position.x;
		float y = rigid.position.y;

		// Check right and left sides.
		if (x <= boundary.xMin || x >= boundary.xMax) 
		{
			rigid.velocity = new Vector2(-rigid.velocity.x,rigid.velocity.y);
		}
		// Check top and bottom sides.
		if (y <= boundary.yMin || y >= boundary.yMax) 
		{
			rigid.velocity = new Vector2(rigid.velocity.x,-rigid.velocity.y);
		}
	}
}
