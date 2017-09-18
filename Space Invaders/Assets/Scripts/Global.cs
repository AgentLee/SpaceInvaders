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

	public float hordeTimer;

	public GameObject safeZone;

	// Toggle this whenever the player loses a life
	// so the player and the enemies don't move/fire.
	public bool freeze;

	public GameObject[] enemy10Holder;
	public GameObject enemy10;

	public bool overExtended;

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

		safeZone = GameObject.Find ("SafeZone").gameObject;


		// Set the high score to 0 if it's the first time playing on the machine.
		highScore.text = PlayerPrefs.GetInt ("HighScore", 0).ToString();

		accuracy.enabled = false;
		accuracy.text = "Accuracy: 0.00%";

		baseText.enabled = false;
		baseText.text = "Base Health: 100.00%";

		hitEnemy = false;

		hordeTimer = 10;

		hordeStart = false;

		overExtended = false;
	}
		
	public bool hordeStart;
	public float farLeftX = -17.51f;
	public float farLeftY = -10.5f;
	public float farLeftZ = -0.00249958f;

	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		hordeTimer -= Time.deltaTime;
		if (hordeTimer < 0) {
			hordeStart = !hordeStart;

			if (hordeStart) {
				// This goes with the aesthetic 
				enemies.transform.position += Vector3.down * 3.0f;

				Vector3 pos = enemy10.transform.position;
				pos.y -= 4;
				Instantiate (enemy10, pos, enemy10.transform.rotation);

				hordeTimer = 30;
			} else {
//				Vector3 pos = enemy10.transform.position;
//				pos.y = 27;
//				while (true) {
//					enemy10.transform.position = Vector3.Lerp (enemy10.transform.position, pos, Time.deltaTime);
//
//					if (Mathf.Abs(enemy10.transform.position.y - pos.y) <= 0.75f) {
//						break;
//					}
//				}
			}

			// TODO
			// Add Winning Condition here
		}

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

			Debug.Log (hitRedUFO);
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
		if (lostLife || overExtended) {
			AudioSource.PlayClipAtPoint (wilhelm, player.transform.position);

			// Destroy one by one
			foreach (Transform life in lives) {
				Destroy (life.gameObject);
				break;
			}

			StartCoroutine ("Respawn");
			lostLife = false;
			overExtended = false;
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

		// Reset player rotation
		player.transform.rotation = Quaternion.identity;

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

		player.GetComponent<PlayerController> ().prepForLaunch = true;

		Debug.Log ("Player can move around for 20 seconds");

		// Need to test the invincibility time a bit.
		yield return new WaitForSeconds (5);

		StartCoroutine ("ReturnToSafeZone");
	}

	IEnumerator ReturnToSafeZone()
	{
		MeshRenderer render = safeZone.gameObject.GetComponentInChildren<MeshRenderer> ();
		render.enabled = false;

		Debug.Log ("10 seconds to return to base");
		render.enabled = true;

		yield return new WaitForSeconds (5);

		Debug.Log ("IF NOT AT BASE, DESTROY");

		// Maybe keep the player out there as a punishment?

		// Check to see where the player is on the screen.
		// If they're in the safe zone they get reset to the start position.
		Vector3 safeZoneCoords = new Vector3(42f, -10f, 10.06f);
		Vector3 startPos = new Vector3(0f, -12.05f, 10.06f);
		Vector3 playerPos = player.transform.position;

		// Player is in the safeZone
		if (playerPos.x <= safeZoneCoords.x && playerPos.x >= -safeZoneCoords.x &&
			playerPos.y <= safeZoneCoords.y && playerPos.y >= -13.5 &&
			playerPos.z == safeZoneCoords.z) {

			invincible = false;
			render.enabled = false;

			// Reset flags
			hitRedUFO = false;
			spawnedRedUFO = false;

			yield break;

			/*
			float distance = Vector3.Distance (startPos, playerPos);
			Debug.Log (distance);

			if (distance <= 2.5f) {
				invincible = false;
				render.enabled = false;

				//		player.GetComponent<PlayerController> ().reset = true;

				// Reset flags
				hitRedUFO = false;
				spawnedRedUFO = false;

				yield break;
			} else {
				player.transform.position = Vector3.Lerp (player.transform.position, startPos, Time.deltaTime);
			}
			*/
		} else {
			Debug.Log ("Destroy Player");
			overExtended = true;
		}
	}
}
