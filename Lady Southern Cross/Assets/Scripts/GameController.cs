using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField]
	GameObject player = null, enemy = null, enemy2 = null, enemy3 = null, boss1 = null, 
				powerUpSpread = null, powerUpHp = null, enemyExplosion = null, powerUpPickUp = null, 
				boss1Explosion = null, playerExplosion = null, playerHit = null, music = null;

	// Used for mute.
	GameObject enemy2Clone, enemy3Clone, boss1Clone, musicButton, soundButton;

	// Pause.
	bool isPaused;
	public Canvas pauseMenu;
	bool isSoundMuted;
	bool isMusicMuted;

	// Game Over.
	public bool isGameOver;
	public Canvas endMenu;

	// Victory.
	public bool victory;
	public Canvas victoryMenu;

	// Texts.
	public GUIText scoreText;
	public GUIText playerHealthText;
	public GUIText powerUpTimerText;
	public GUIText powerUpAmountText;
	public GUIText bossWarningText;
	public Text gameOverScoreText;
	public Text victoryScoreText;

	public int enemyCount;
	public bool isBossSpawned;

	// Spawnvalues.
	public Vector3 spawnValues;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	private Vector3 spawnPosition;
	private Quaternion spawnRotation;

	private int score;
	private int playerHealth;
	private float spreadTimer;
	private int powerUpAmount;


	void Start()
	{

		// Make sure it is unpaused and menus are disabled.
		isPaused = false;
		pauseMenu.enabled = false;
		endMenu.enabled = false;
		victoryMenu.enabled = false;
		isBossSpawned = false;
		Time.timeScale = 1;

		// Enable music and sound controlls.
		music = GameObject.FindGameObjectWithTag ("Music");
		powerUpPickUp = GameObject.FindGameObjectWithTag ("PowerUpPickUp");
		music.GetComponent<AudioSource> ().enabled = true;
		music.GetComponent<AudioSource> ().Play();
		enemy2.GetComponent<AudioSource> ().mute = false;
		enemy3.GetComponent<AudioSource> ().mute = false;
		boss1.GetComponent<AudioSource> ().mute = false;

		// Pausemenu buttons.
		soundButton = GameObject.FindGameObjectWithTag ("SoundButton");
		musicButton = GameObject.FindGameObjectWithTag ("MusicButton");

		//Spawns the player.
		spawnRotation = Quaternion.identity;
		Instantiate(player, new Vector3(0, -4, -1), spawnRotation);

		// Enables the player controlls.
		player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<PlayerController> ().enabled = true;

		// Resets the HUD.
		playerHealth = 1;
		playerHealthText.text = "[ HP ]> " + playerHealth + "/5";
		score = 0;
		scoreText.text = "[ Score ]> " + score;
		powerUpAmount = 0;
		powerUpAmountText.text = powerUpAmount + "/3 <[ Power Up ]";
		spreadTimer = 0;
		powerUpTimerText.text = "Not active <[ Timer ]";

		// Start the enemywaves.
		StartCoroutine (SpawnEnemyWaves ());

	}

	void Update()
	{

		// Pause
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if(isPaused)
			{
				UnPause();
			}
			else
			{
				Pause();
			}
		}

		if (isGameOver) 
		{
			StartCoroutine(GameOver());
		}

		if (victory) 
		{
			StartCoroutine(Victory());
		}
	}

	public void Pause()
	{
		if (player == null) 
		{
			Debug.Log ("Cannot find 'Player'");
		} 
		else 
		{
			// Disables the player controlls.
			player.GetComponent<PlayerController> ().enabled = false;

			// Lower the volume.
			music.GetComponent<AudioSource> ().volume = music.GetComponent<AudioSource> ().volume = 0.1f;

			// Brings up the menu.
			pauseMenu.enabled = true;
			isPaused = true;

			// Pauses the game.
			Time.timeScale = 0;
		}
	}

	public void UnPause()
	{
		if (player == null) 
		{
			Debug.Log ("Cannot find 'Player'");
		} 
		else 
		{
			// Enables the player controlls.
			player.GetComponent<PlayerController> ().enabled = true;

			// Resets the volume.
			music.GetComponent<AudioSource> ().volume = 0.2f;

			// Removes the menu.
			pauseMenu.enabled = false;
			isPaused = false;

			// Removes the pause.
			Time.timeScale = 1;
		}
	}

	public void Restart()
	{
		Application.LoadLevel ("Main");
	}

	public void MainMenu()
	{
		Application.LoadLevel ("Start");
	}

	IEnumerator GameOver()
	{
		gameOverScoreText.text = "Score: " + score;
		yield return new WaitForSeconds (2);
		endMenu.enabled = true;
		Time.timeScale = 0;
		
	}

	IEnumerator Victory()
	{
		victoryScoreText.text = "Score: " + score;
		yield return new WaitForSeconds (2);
		victoryMenu.enabled = true;
		Time.timeScale = 0;
	}

	public void MuteSound()
	{
		// Sound mute.
		if (isSoundMuted == false) 
		{
			// Player mute.
			player.GetComponent<AudioSource> ().mute = true;
			playerExplosion.GetComponent<AudioSource> ().mute = true;
			playerHit.GetComponent<AudioSource> ().mute = true;

			// Enemy2 mute.
			if(enemy2Clone != null)
			{
				enemy2Clone.GetComponent<AudioSource> ().mute = true;
			}
			else
			{
				enemy2.GetComponent<AudioSource>().mute = true;
			}

			// Enemy3 mute.
			if(enemy3Clone != null)
			{
				enemy3Clone.GetComponent<AudioSource> ().mute = true;
			}
			else
			{
				enemy3.GetComponent<AudioSource>().mute = true;
			}

			// Boss mute.
			if(boss1Clone != null)
			{
				boss1Clone.GetComponent<AudioSource>().mute = true;
			}
			else
			{
				boss1.GetComponent<AudioSource>().mute = true;
			}

			// EnemyExplosion mute.
			if(enemyExplosion != null)
			{
				enemyExplosion.GetComponent<AudioSource>().mute = true;
			}
			if(boss1Explosion != null)
			{
				boss1Explosion.GetComponent<AudioSource>().mute = true;
			}
			if(playerExplosion != null)
			{
				playerExplosion.GetComponent<AudioSource>().mute = true;
			}

			soundButton.GetComponent<Button>().image.color = Color.grey;
			isSoundMuted = true;
		} 

		// Sound unmute.
		else 
		{
			// Player unmute.
			player.GetComponent<AudioSource> ().mute = false;
			playerExplosion.GetComponent<AudioSource> ().mute = false;
			playerHit.GetComponent<AudioSource> ().mute = false;

			// Enemy2 unmute.
			if(enemy2Clone != null)
			{
				enemy2Clone.GetComponent<AudioSource> ().mute = false;
			}
			else
			{
				enemy2.GetComponent<AudioSource>().mute = false;
			}
		
			// Enemy3 unmute.
			if(enemy3Clone != null)
			{
				enemy3Clone.GetComponent<AudioSource> ().mute = false;
			}
			else
			{
				enemy3.GetComponent<AudioSource>().mute = false;
			}

			// Boss unmute.
			if(boss1Clone != null)
			{
				boss1Clone.GetComponent<AudioSource>().mute = false;
			}
			else
			{
				boss1.GetComponent<AudioSource>().mute = false;
			}

			// EnemyExplosion unmute.
			if(enemyExplosion != null)
			{
				enemyExplosion.GetComponent<AudioSource>().mute = false;
			}
			if(boss1Explosion != null)
			{
				boss1Explosion.GetComponent<AudioSource>().mute = false;
			}
			if(playerExplosion != null)
			{
				playerExplosion.GetComponent<AudioSource>().mute = false;
			}

			soundButton.GetComponent<Button>().image.color = Color.white;
			isSoundMuted = false;
		}
	}

	public void MuteMusic()
	{
		if (isMusicMuted == false) 
		{
			music.GetComponent<AudioSource> ().mute = true;
			musicButton.GetComponent<Button>().image.color = Color.grey;
			isMusicMuted = true;
		}
		else 
		{
			music.GetComponent<AudioSource> ().mute = false;
			musicButton.GetComponent<Button>().image.color = Color.white;
			isMusicMuted = false;
		}
	}

	// Enemy1Ship. Spawning the small suicide enemies.
	IEnumerator SpawnEnemyWaves()
	{
		int waveCounter = 0;

		yield return new WaitForSeconds(startWait);
		while (true) 
		{
			for (int i = 0; i < enemyCount; i++) 
			{
				spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Instantiate(enemy, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(spawnWait);
			}

			yield return new WaitForSeconds (waveWait); 
			waveCounter++;

			// Boss1. Spawns the first boss.
			if(waveCounter%10 == 0)
			{
				for(int i = 0; i < 3; i++)
				{
					yield return new WaitForSeconds(0.8f);
					bossWarningText.enabled = true;
					yield return new WaitForSeconds(0.8f);
					bossWarningText.enabled = false;
				}
				StopCoroutine(SpawnEnemyWaves());
				SpawnBoss1();
				isBossSpawned = true;
				break;
			}

			// Enemy3. Spawns the third enemy.
			if(waveCounter%4 == 0)
			{
				SpawnThirdEnemy();
			}

			// Enemy2. Spawns the second enemy.
			if(waveCounter%2 == 0)
			{
				SpawnSecondEnemy();
			}

		}
	} 

	// Enemy2Ship. Spawning the second enemy which fires laser.
	void SpawnSecondEnemy()
	{
		spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		enemy2Clone = (GameObject)Instantiate(enemy2, spawnPosition, spawnRotation);
		enemy2Clone = GameObject.FindGameObjectWithTag ("Enemy2");

	}

	// Enemy3Ship. Spawning the third enemy which fires laser and go back and forth in a pattern across the gameboard.
	void SpawnThirdEnemy()
	{
		spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		enemy3Clone = (GameObject)Instantiate (enemy3, spawnPosition, spawnRotation);
		enemy3Clone = GameObject.FindGameObjectWithTag ("Enemy3");

	}

	void SpawnBoss1()
	{
		spawnPosition = new Vector3(0, spawnValues.y, spawnValues.z);
		boss1Clone = (GameObject)Instantiate(boss1, spawnPosition, spawnRotation);
		boss1Clone = GameObject.FindGameObjectWithTag ("Boss1");
	}

	// Spread powerup.
	public void PowerUpSpread(GameObject enemyPosition)
	{
		/*GameObject clonePowerUp = (GameObject)*/Instantiate (powerUpSpread, 
                       					new Vector3(enemyPosition.transform.position.x,
		            								enemyPosition.transform.position.y),
                   						Quaternion.identity);
	}

	// Hp powerup.
	public void PowerUpHp(GameObject enemyPosition)
	{
		/*GameObject clonePowerUp = (GameObject)*/Instantiate (powerUpHp, 
                      					new Vector3(enemyPosition.transform.position.x,
		            								enemyPosition.transform.position.y),
                                  	 	Quaternion.identity);
	}

	public void PlaySound(string tag)
	{
		if (!isSoundMuted) 
		{
			if (tag == "Boss1") 
			{
				boss1Explosion.GetComponent<AudioSource> ().Play ();
			} 
			if(tag == "Enemy" || tag == "Enemy2" || tag == "Enemy3") 
			{
				enemyExplosion.GetComponent<AudioSource> ().Play ();
			}
			if(tag == "PlayerHit")
			{
				playerHit.GetComponent<AudioSource> ().Play();
			}
			if(tag == "PlayerExplosion")
			{
				playerExplosion.GetComponent<AudioSource> ().Play();
			}
			if(tag == "PowerUp")
			{
				powerUpPickUp.GetComponent<AudioSource> ().Play ();
			}
		}
	}

	// Score.
	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	// ScoreText.
	void UpdateScore()
	{
		scoreText.text = "[ Score ]> " + score;
	}

	// PlayerHealth.
	public void PlayerHealth (int newPlayerHealth)
	{
		playerHealth = newPlayerHealth;
		UpdatePlayerHealth ();
	}

	// PlayerHealthText.
	void UpdatePlayerHealth()
	{
		playerHealthText.text = "[ HP ]> " + playerHealth + "/5";
	}

	// PowerUpAmount.
	public void PowerUpAmount (int newPowerUpAmount)
	{
		powerUpAmount = newPowerUpAmount;
		UpdatePowerUpAmount();
	}

	// PowerUpAmountText.
	void UpdatePowerUpAmount()
	{
		powerUpAmountText.text = powerUpAmount + "/3 <[ Power Up ]";
	}

	// PowerUpTimer.
	public void PowerUpTimer(float timer)
	{
		Mathf.Floor(spreadTimer = timer);
		if (timer <= 0) 
		{
			powerUpTimerText.text = "Not active <[ Timer ]";
			return;
		}
		UpdatePowerUpTimer ();
	}
	// PowerUpTimerText.
	void UpdatePowerUpTimer()
	{
		powerUpTimerText.text = Mathf.Floor(spreadTimer) + " <[ Timer ]";
	}
}
