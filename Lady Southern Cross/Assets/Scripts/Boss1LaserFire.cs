using UnityEngine;
using System.Collections;

public class Boss1LaserFire : MonoBehaviour {

	// Player
	public Transform target;
	
	// Weapon
	public GameObject shot;
	public Transform[] shotSpawn;
	public int shotsInBurst;
	public float fireRate;
	private float nextFire;
	private int wavesFired;

	void Start()
	{

		target = GameObject.FindGameObjectWithTag ("Player").transform;
		StartCoroutine (Fire ());
	}

	IEnumerator Fire()
	{
		while (true) 
		{
			yield return new WaitForSeconds (3);
			if (Time.time > nextFire) 
			{
				nextFire = Time.time + fireRate;

				for (int i = 0; i < shotsInBurst; i++) 
				{
					GetComponent<AudioSource> ().Play ();
					for (int j = 0; j < shotSpawn.Length; j++) 
					{
						Instantiate (shot, shotSpawn [j].position, shotSpawn [j].rotation);
					}
					yield return new WaitForSeconds (0.1f);
				}
				wavesFired++;
			}

			if(wavesFired == 3)
			{
				wavesFired = 0;
				break;
			}
		}
		StartCoroutine(AlternativeFire());
	}

	IEnumerator AlternativeFire()
	{
		Quaternion standardRotation = shotSpawn[0].rotation;
		yield return new WaitForSeconds (3);
		while (true) 
		{
			shotSpawn [0].Rotate(Vector3.forward *(Random.Range (-90f, 90f)));
			Instantiate (shot, shotSpawn [0].position, shotSpawn [0].rotation);
			shotSpawn[0].rotation = standardRotation;
			yield return new WaitForSeconds (0.05f);

			wavesFired++;

			if(wavesFired == 80)
			{
				wavesFired = 0;
				shotSpawn[0].rotation = standardRotation;
				StartCoroutine(Fire());
				break;
			}
		}
	}
}
