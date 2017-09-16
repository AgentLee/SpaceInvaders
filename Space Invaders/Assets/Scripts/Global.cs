using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour 
{
	// UI/Sound Elements
	public float timer;
	public int score;
	public int[] scores;
	public Text highScore;
	public GameObject gameOver;
	public GameObject levelUp;
	public GameObject livesObject;
//	public AudioSource audio;

	// Player Management
	public GameObject player;
	public bool lostLife;
	public int numLives;
	public Transform extraLife;
	public Vector3 rotateAmount;
	// TODO
	// Find a better way to delete bases
	public int baseCount;
	public Text baseText;
	public AudioClip wilhelm;
	public bool hitEnemy;
	public Text accuracy;

	// Enemy Management
	public GameObject enemies;
	public int numEnemies;
	// Red UFO
	public GameObject redUFO;
	public bool hitRedUFO;
	public float maxTime = 5;
	public float minTime = 2;
	private float timeRedUFO;
	private float spawnTime;
	public bool spawnedRedUFO;

	// Toggle this whenever the player loses a life
	// so the player and the enemies don't move/fire.
	public bool freeze;

	// Use this for initialization
	void Start ()
	{
		// Hide the cursor
		Cursor.visible = false;

		invincible = false;

		// UI
		scores = new int[5];
		score = 0;
		timer = 0;

		// Player
		lostLife = false;
		numLives = 3;
		baseCount = 40;

		// Enemy
		// Should be able to create enemies in a loop rather than
		// having them static in the editor.
		numEnemies = 33;

		// Lock player/enemy movement
		freeze = false;

		// Red UFO
		SetRandomTime ();
		spawnedRedUFO = false;
		hitRedUFO = false;

		// Hide the end game conditions
		livesObject = GameObject.Find ("Lives").gameObject;

		gameOver = GameObject.Find ("GameOver").gameObject;
		gameOver.GetComponent<Text> ().enabled = false;

		levelUp = GameObject.Find ("LevelUp").gameObject;
		levelUp.GetComponent<Text> ().enabled = false;

		// Set the high score to 0 if it's the first time playing on the machine.
		highScore.text = PlayerPrefs.GetInt ("HighScore", 0).ToString();

		accuracy.enabled = false;
		accuracy.text = "Accuracy: 0.00%";

		baseText.enabled = false;
		baseText.text = "Base Health: 100.00%";

		hitEnemy = false;
	}

	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		// Checks for
		// 		- Player loses all their lives
		// 		- All the bases are destroyed
		//		- The enemies reached the bases
		//
		if (CheckEndGame ()) {
			return;
		}

		timeRedUFO += Time.deltaTime;
		if (timeRedUFO >= spawnTime) {
			SpawnRedUFO ();
			SetRandomTime ();
		}

		// Rotate the lives at the bottom left
		RotateLives (livesObject.transform);
		// Check to see if the player died and needs to respawn
		CheckRespawn (livesObject.transform);

		// TODO
		CheckLevelUp();

		UpdateHighScore ();

		accuracy.text = "Accuracy: " + player.gameObject.GetComponent<PlayerController> ().accuracy.ToString ("F2") + "%";
		if (Input.GetKey (KeyCode.Tab)) {
			accuracy.enabled = true;
		} else {
			accuracy.enabled = false;
		}

		if (hitEnemy) {
			hitEnemy = false;

			player.gameObject.GetComponent<PlayerController> ().shotsHit++;
		}
	
		if (hitRedUFO) {
			StartCoroutine ("RedDeadUFO");
		}

//		Debug.Log ("SPAWNED: " + spawnedRedUFO);
	}

	// ---------------------------------------------------------------
	// End Game Conditions
	// ---------------------------------------------------------------

	bool EnemiesReachedBase()
	{
		return enemies.transform.position.y <= 1.0f;
	}

	bool RanOutOfLives()
	{
		if (lostLife && numLives == 0) {
			AudioSource.PlayClipAtPoint (wilhelm, player.transform.position);

			foreach (Transform life in livesObject.transform) {
				Destroy (life.gameObject);
				break;
			}

			return true;
		}

		return false;
	}

	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject enemy4;
	bool CheckEndGame()
	{
		if (baseCount == 0 || EnemiesReachedBase () || RanOutOfLives()) {
			StartCoroutine ("GG");

			gameOver.GetComponent<Text> ().enabled = true;

			return true;
		}

		return false;
	}

	Vector3 RandomPosition()
	{
		return new Vector3 (Random.Range (-30, 30), Random.Range (-30, 30), Random.Range (-10, 10));
	}

	// ---------------------------------------------------------------
	// Lives Management
	// --------------------------------------------------------------

	void RotateLives(Transform lives)
	{
		foreach (Transform life in lives) {
			life.Rotate (rotateAmount * Time.deltaTime);
		}
	}

	void CheckRespawn(Transform lives)
	{
		if (lostLife) {
			AudioSource.PlayClipAtPoint (wilhelm, player.transform.position);

			// Destroy one by one
			foreach (Transform life in lives) {
				Destroy (life.gameObject);
				break;
			}

			StartCoroutine ("Respawn");
			lostLife = false;
		}
	}

	void UpdateHighScore()
	{
		if (Input.GetKey (KeyCode.R)) {
			PlayerPrefs.SetInt ("HighScore", 0);
			highScore.text = score.ToString();
		}

		if (score > PlayerPrefs.GetInt ("HighScore", 0)) {
			PlayerPrefs.SetInt ("HighScore", score);
			highScore.text = score.ToString ();
		}

		// TODO
//		int scorePosition = 0;
//		if (score > PlayerPrefs.GetInt ("HighScore5", 0)) {
//			scorePosition = 5;
//			if (score > PlayerPrefs.GetInt ("HighScore4", 0)) {
//				scorePosition = 4;
//				if (score > PlayerPrefs.GetInt ("HighScore3", 0)) {
//					scorePosition = 3;
//					if (score > PlayerPrefs.GetInt ("HighScore2", 0)) {
//						scorePosition = 2;
//						if (score > PlayerPrefs.GetInt ("HighScore", 0)) {
//							scorePosition = 1;
//						} 
//					}
//				}
//			}
//		}
//
//		if (scorePosition > 1) {
//			PlayerPrefs.SetInt ("HighScore" + scorePosition, score);
//			Debug.Log ("NEW HIGH SCORE AT " + scorePosition);
//		} 
//		else {
//			PlayerPrefs.SetInt ("HighScore", score);
//			highScore.text = score.ToString ();
//		}



//		for (int i = 0; i < scores.Length; i++) {
//			int highScoreKey = "HighScore" + (i + 1).ToString ();
//			int highScore = PlayerPrefs.SetInt (highScoreKey, 0);
//
//			if (score > highScore) {
//				int temp = highScore;
//				PlayerPrefs.SetInt (highScoreKey, score);
//				score = temp;
//			}
//		}
//
//		Debug.Log (scores);
	}

	void CheckLevelUp()
	{
		// Level Up
		if (numEnemies <= 0) {
			levelUp.GetComponent<Text> ().enabled = true;

			numLives++;

			// TODO
			// Fix extra life instantiation
			//		g = GameObject.FindGameObjectWithTag ("Lives");
			//		Vector3 lifePos = new Vector3(0, 0, 0);
			//		Quaternion lifeRotation = Quaternion.identity;
			//		foreach (Transform life in g.transform) {
			//			lifePos = life.position;
			//			lifeRotation = life.rotation;
			//		}

			//		Instantiate (extraLife, lifePos, lifeRotation);
			//Debug.Log (g.transform.childCount);
		}
	}

	// ---------------------------------------------------------------
	// Red UFO Management
	// ---------------------------------------------------------------

	void SpawnRedUFO()
	{
		timeRedUFO = 0;

		if (!spawnedRedUFO) {
			Instantiate (redUFO, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity);
		}
	}

	void SetRandomTime()
	{
		spawnTime = Random.Range (minTime, maxTime);
	}

	// ---------------------------------------------------------------
	// Wait Functions
	// ---------------------------------------------------------------

	IEnumerator Respawn()
	{
		// Make sure nothing moves/fires
		freeze = true;

		// Hide the player
		// EnemyBulletController handles the explosion effect
		MeshRenderer render = player.gameObject.GetComponentInChildren<MeshRenderer> ();
		render.enabled = false;

		// Respawn time
		yield return new WaitForSeconds (1);

		// Flash the player
		float timeToBlink = Time.time + 3;
		while (Time.time < timeToBlink) {
			yield return new WaitForSeconds (0.5f);
			 
			render.enabled = !render.enabled;
		}

		// Just to make sure the player is shown
		render.enabled = true;

		// Allow the player/enemies to move
		freeze = false;
	}

	IEnumerator GG()
	{
		DestroyObject (player);

		if (EnemiesReachedBase()) {
			for (int i = 0; i < 5; i++) {
				Instantiate (enemy1, RandomPosition(), Quaternion.identity);
				Instantiate (enemy2, RandomPosition(), Quaternion.identity);
				Instantiate (enemy3, RandomPosition(), Quaternion.identity);
				Instantiate (enemy4, RandomPosition(), Quaternion.identity);

				StartCoroutine ("DestroyEnemies");

				DestroyObject (enemy1);
				DestroyObject (enemy2);
				DestroyObject (enemy3);
				DestroyObject (enemy4);
			}
		}

		yield return new WaitForSeconds (10);

		Time.timeScale = 0;
	}

	IEnumerator DestroyEnemies()
	{
		yield return new WaitForSeconds (3);
	}

	public bool invincible;
	IEnumerator RedDeadUFO()
	{
		invincible = true;
		Debug.Log ("player can move all around the screen");

		// Need to test the invincibility time a bit.
		yield return new WaitForSeconds (10);

		invincible = false;
		Debug.Log ("reset player position now");

		player.GetComponent<PlayerController> ().reset = true;

		// Reset flags
		hitRedUFO = false;
		spawnedRedUFO = false;
	}
}
