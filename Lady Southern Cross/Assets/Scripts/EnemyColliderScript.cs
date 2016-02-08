using UnityEngine;
using System.Collections;

public class EnemyColliderScript : MonoBehaviour {

	public int scoreValue;
	public int enemyHealth;

	// Explosions
	public GameObject EnemyExplosion;
	public float explosionTime;

	private GameController gameController;
	private PlayerController playerController;
	
	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		GameObject playerControllerObject = GameObject.FindWithTag ("Player");

		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (playerControllerObject != null) 
		{
			playerController = playerControllerObject.GetComponent<PlayerController>();
		}
		
		if (gameController == null) 
		{
			Debug.Log ("Cannot find 'GameController' script");
		}

		if (playerController == null) 
		{
			Debug.Log ("Cannot find 'playerController' script");
		}
	}
	
	
	
	// Destroy everything that enters the trigger...
	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log(enemyHealth);
		// ...except these tags.
		if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Enemy2" || other.tag == "Enemy3" || other.tag == "Boss1" || 
		    other.tag == "EnemyLaser" || other.tag == "Boss1Fire" || other.tag == "PowerUpSpread" || other.tag == "PowerUpHp") 
		{
			return;
		}

		if (other.tag == "Player" && tag != "Boss1") 
		{
			enemyHealth = 0;
		}

		enemyHealth --;

		if (enemyHealth <= 0) 
		{
			if(tag == "Boss1")
			{
				gameController.PlaySound(tag);
				gameController.victory = true;
			}

			// If enemy collide, this is the enemy explosion.
			GameObject cloneEnemyExplosion = (GameObject)Instantiate (EnemyExplosion, 
			                                                          new Vector3 (gameObject.GetComponent<Rigidbody2D> ().position.x, 
			             gameObject.GetComponent<Rigidbody2D> ().position.y), 
			                                                          Quaternion.identity);

		// Spawns hp powerup with a droprate of 1 in 20.
			if(tag == "Enemy")
			{
				int spawnHp = Random.Range(1, 20);
				if(spawnHp == 1)
				{
					gameController.PowerUpHp(cloneEnemyExplosion);
				}
			}

		// Spawns spread powerup from enemy2 with a 50% droprate.
			if(tag == "Enemy2")
			{
				int spawnSpread = Random.Range(1, 2);
				if(spawnSpread == 1)
				{
					gameController.PowerUpSpread(cloneEnemyExplosion);
				}
			}

		// Spawns spread powerup from enemy3 with a 100% droprate.
			if(tag == "Enemy3")
			{
				gameController.PowerUpSpread(cloneEnemyExplosion);
			}

			gameController.AddScore (scoreValue);
			gameController.PlaySound("Enemy");
			Destroy (cloneEnemyExplosion, explosionTime);
			Destroy (gameObject);
		}

		if (other.tag == "PlayerLaser") 
		{
			Destroy (other.gameObject);
		}
	}
}
