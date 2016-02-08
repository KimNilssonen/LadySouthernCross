using UnityEngine;
using System.Collections;

public class EnemyLaserFire : MonoBehaviour 
{
	// Player
	public Transform target;

	// Weapon
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire;
	public int shotsInBurst;

	void Start()
	{
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update()
	{	
		
		if (target != null) 
		{
			// ShotSpawn rotates so that forward is toward the player.
			var lookDir = target.position - shotSpawn.position;
			float angle = Mathf.Atan2 (lookDir.y, lookDir.x) * Mathf.Rad2Deg;
			shotSpawn.rotation = Quaternion.Euler (0f, 0f, angle + 90);

			// Enemy3 fires in bursts.
			if(gameObject.tag == "Enemy3")
			{
				if(Time.time > nextFire)
				{
					nextFire = Time.time + fireRate;
					StartCoroutine(Fire_Enemy3());	
				}
			}
			else
			{
				StartCoroutine(Fire ());
			}
		} 

		else 
		{
			Debug.Log ("No Target!");
		}
	}

	// Normal fire.
	IEnumerator Fire()
	{
		yield return new WaitForSeconds (2);
		if (Time.time > nextFire) 
		{
			GetComponent<AudioSource>().Play();
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		}
	}

	// Fires bursts.
	IEnumerator Fire_Enemy3()
	{
		yield return new WaitForSeconds (0.5f);

		for (int i = 0; i < shotsInBurst; i++) 
		{
				GetComponent<AudioSource>().Play();
				Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				yield return new WaitForSeconds(0.2f);
		}
	}
}
