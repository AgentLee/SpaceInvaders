using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour 
{
	// UI Elements
	public float timer;
	public int score;
	public Text highScore;
	public GameObject gameOver;
	public GameObject levelUp;
	public GameObject livesObject;

	// Enemy Management
	public GameObject enemies;
	public int numEnemies;

	// Player Management
	public GameObject player;
	public bool lostLife;
	public int numLives;
	public Transform extraLife;
	public Vector3 rotateAmount;
	// TODO
	// Find a better way to delete bases
	public int baseCount;

	// Toggle this whenever the player loses a life
	// so the player and the enemies don't move/fire.
	public bool freeze;
	public AudioClip explosion;

	// Use this for initialization
	void Start ()
	{
		// Hide the cursor
		Cursor.visible = false;

		score = 0;
		timer = 0;

		// Should be able to create enemies in a loop rather than
		// having them static in the editor.
		numEnemies = 33;

		lostLife = false;
		numLives = 3;
		baseCount = 40;

		freeze = false;

		// Hide the end game conditions
		livesObject = GameObject.Find ("Lives").gameObject;

		gameOver = GameObject.Find ("GameOver").gameObject;
		gameOver.GetComponent<Text> ().enabled = false;

		levelUp = GameObject.Find ("LevelUp").gameObject;
		levelUp.GetComponent<Text> ().enabled = false;

		// Set the high score to 0 if it's the first time playing on the machine.
		highScore.text = PlayerPrefs.GetInt ("HighScore", 0).ToString();
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

		// Rotate the lives at the bottom left
		RotateLives (livesObject.transform);
		// Check to see if the player died and needs to respawn
		CheckRespawn (livesObject.transform);

		// TODO
		CheckLevelUp();

		UpdateHighScore ();
	}
		
	bool EnemiesReachedBase()
	{
		return enemies.transform.position.y <= 1.0f;
	}

	bool RanOutOfLives()
	{
		if (lostLife && numLives == 0) {
			return true;
		}

		return false;
	}

	bool CheckEndGame()
	{
		if (baseCount == 0 || EnemiesReachedBase () || RanOutOfLives()) {
			StartCoroutine ("GG");

			gameOver.GetComponent<Text> ().enabled = true;

			return true;
		}

		return false;
	}

	void RotateLives(Transform lives)
	{
		foreach (Transform life in lives) {
			life.Rotate (rotateAmount * Time.deltaTime);
		}
	}

	void CheckRespawn(Transform lives)
	{
		if (lostLife) {
			// Destroy one by one
			foreach (Transform life in lives) {
				Destroy (life.gameObject);
				break;
			}

			StartCoroutine ("Respawn");
			lostLife = false;

			AudioSource.PlayClipAtPoint (explosion, gameObject.transform.transform.position);
		}
	}

	void UpdateHighScore()
	{
		if (score > PlayerPrefs.GetInt ("HighScore", 0)) {
			PlayerPrefs.SetInt ("HighScore", score);
			highScore.text = score.ToString();
		}
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

		yield return new WaitForSeconds (10);

		Time.timeScale = 0;
	}
}
