using UnityEngine;
using System.Collections;

public class PlayerLaserCollision : MonoBehaviour 
{
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "EnemyLaser" || other.tag == "Boss1Fire" || other.tag == "Player" || other.tag == "PowerUpSpread") 
		{
			return;
		}
	}
}
