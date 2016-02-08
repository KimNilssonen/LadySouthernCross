using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMax, xMin, yMax, yMin;
}

public class PlayerController : MonoBehaviour 
{
	[SerializeField]
	float speed = 0;
	public Boundary boundary;
	
	// Weapon
	public GameObject shot;
	public Transform shotSpawn;
	public Transform shotSpawn2;
	public Transform shotSpawn3;
	public Transform shotSpawn4;
	public float fireRate;
	private float nextFire;

	// Power ups.
	public bool isPowerUpSpread;
	private int powerUpAmount;
	private int maxPowerUps = 3;
	private float spreadTimer = 20;

	// Player props.
	public int playerHealth;
	public float explosionTime;
	public GameObject PlayerExplosion;
	private int maxHp = 5;

	private GameController gameController; 


	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");


		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) 
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void Update()
	{
		// Activates the powerup.
		if(Input.GetKeyUp(KeyCode.LeftControl) && powerUpAmount > 0)
	   	{
			if(!isPowerUpSpread)
			{
				isPowerUpSpread = true;
			}
				powerUpAmount--;
				gameController.PowerUpAmount(powerUpAmount);
				spreadTimer = 20;

		}

		// Fires laser.
		if(Input.GetButton("Jump") && Time.time > nextFire)
	    {
			GetComponent<AudioSource>().Play();
			if(isPowerUpSpread)
			{
				nextFire = Time.time + fireRate;
				Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
				Instantiate(shot, shotSpawn3.position, shotSpawn3.rotation);
				Instantiate(shot, shotSpawn4.position, shotSpawn4.rotation);
			}
			else
			{
				nextFire = Time.time + fireRate;
				Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
			}

		}

		// Update the timer if the powerup is active.
		if (isPowerUpSpread) 
		{
			spreadTimer = spreadTimer - (Time.deltaTime * 1);
			gameController.PowerUpTimer(spreadTimer);
			if(spreadTimer <= 0)
			{
				isPowerUpSpread = false;
				spreadTimer = 20;
			}
		}
	}

	// Physics
	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		GetComponent<Rigidbody2D>().velocity = movement * speed;

		GetComponent<Rigidbody2D> ().position = new Vector2
			(
				Mathf.Clamp(GetComponent<Rigidbody2D> ().position.x, boundary.xMin, boundary.xMax),
				Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax)
			);
	}


	void OnTriggerEnter2D (Collider2D other)
	{

		// This exists so that the player doesn't explode on start.
		if (other.tag == "Boundary" || other.tag == "PlayerLaser") 
		{
			return;
		}

		if (other.tag == "PowerUpSpread") 
		{
			if(powerUpAmount < maxPowerUps)
			{
				gameController.PlaySound("PowerUp");
				Destroy(other.gameObject);
				powerUpAmount++;
				gameController.PowerUpAmount(powerUpAmount);
			}
			return;
		}

		if (other.tag == "PowerUpHp") 
		{
			if(playerHealth < maxHp)
			{
				gameController.PlaySound("PowerUp");
				Destroy(other.gameObject);
				playerHealth++;
				gameController.PlayerHealth (playerHealth);
			}
			return;
		}

		gameController.PlaySound ("PlayerHit");
		playerHealth--;

		if (other.tag == "Boss1") 
		{
			playerHealth = 0;
		}
		if (other.tag == "EnemyLaser" || other.tag == "Boss1Fire") 
		{
			Destroy(other.gameObject);
		}

		gameController.PlayerHealth (playerHealth);

		if (playerHealth <= 0) 
		{
			gameController.PlaySound("PlayerExplosion");
			// Everything other than the Boundary and playerLaser destroys the player.
			GameObject clonePlayerExplosion = (GameObject)Instantiate (PlayerExplosion,
		           new Vector3 (gameObject.GetComponent<Rigidbody2D> ().position.x,
		            			gameObject.GetComponent<Rigidbody2D> ().position.y),
		         	Quaternion.identity);
			Destroy (clonePlayerExplosion, explosionTime);
			Destroy (gameObject);
			gameController.isGameOver = true;
		} 
	}
}
