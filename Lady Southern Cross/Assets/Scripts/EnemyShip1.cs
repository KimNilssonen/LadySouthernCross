using UnityEngine;
using System.Collections;


public class EnemyShip1 : MonoBehaviour
{
	public Boundary boundary;

	// Enemy properties
	public Vector2 speed = new Vector2 ();

	void Start ()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2(Random.Range(-speed.x, speed.x), speed.y);
	}
}
