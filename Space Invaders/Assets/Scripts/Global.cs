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
	public bool invincible;
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

    public GameObject hordeHolder;
	public float hordeTimer;

	public GameObject safeZone;

	// Toggle this whenever the player loses a life
	// so the player and the enemies don't move/fire.
	public bool freeze;

	public GameObject[] enemy10Holder;
	public GameObject enemy10;
	public GameObject enemy20;
	public GameObject enemy30;

	public bool overExtended;
    public bool invincibilityFinished;

    public bool spawnResources;

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

        hordeTimer = 5;
        hordeHolder = GameObject.Find("Horde").gameObject;
        foreach(Transform horder in hordeHolder.transform)
        {
            if(horder.tag == "HordeTimer")
            {
                horder.GetComponent<Text>().text = ": " + hordeTimer.ToString("N0");
            }
        }

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

		overExtended = false;

        invincibilityFinished = false;

        hordeStart = false;

        SpawnEnemies();
	}

    void SpawnEnemies()
    {
        Instantiate(enemy10, enemy10.transform.position, enemy10.transform.rotation);
        Instantiate(enemy20, enemy20.transform.position, enemy20.transform.rotation);
        Instantiate(enemy30, enemy30.transform.position, enemy30.transform.rotation);
    }

    IEnumerator Horder()
    {
        Vector3 newPos = enemy10.transform.position;
        newPos.x = 100;

        float distance = Vector3.Distance(enemy10.transform.position, newPos);

        enemy10.transform.position = Vector3.Lerp(enemy10.transform.position, newPos, Time.deltaTime);

        //if (distance > 1.5f)
        //{
        //}
        //else {
        //    hordeStart = false;

        //    yield break;
        //}

        yield break;
    }

    // Update is called once per frame
    void Update () 
	{
        timer += Time.deltaTime;

        UpdateHordeTimer();
        if(hordeTimer <= 0.0f)
        {
            hordeStart = true;

            // Reset horde timer
            hordeTimer = 30;

            // Move this to coroutine?
            StartHorde();

            hordeStart = false;
        }

        // Checks for
        // 		- Player loses all their lives
        // 		- All the bases are destroyed
        //		- The enemies reached the bases
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

        // See if there's another way without having a new boolean.
        if(invincibilityFinished)
        {
            // BLESS THIS FUNCTION.
            StopCoroutine("RedDeadUFO");

            invincibilityFinished = false;

            // Check to see where the player is on the screen.
            // If they're in the safe zone they get reset to the start position.
            Vector3 safeZoneCoords = new Vector3(42f, -10f, 10.06f);
            Vector3 startPos = new Vector3(0f, -12.05f, 10.06f);
            Vector3 playerPos = player.transform.position;

            // Player is in the safeZone
            if (playerPos.x <= safeZoneCoords.x && playerPos.x >= -safeZoneCoords.x &&
                playerPos.y <= safeZoneCoords.y && playerPos.y >= -13.5 &&
                playerPos.z == safeZoneCoords.z)
            {
                // Move the player return to the starting positionish
                if (Vector3.Distance(playerPos, startPos) > 0.5f) {
                    player.transform.position = Vector3.Lerp(player.transform.position, startPos, Time.deltaTime);
                    invincibilityFinished = true;
                }
            }
            else
            {
                overExtended = true;
            }
        }
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

    public GameObject explosion;
    public GameObject monster;
    public bool collidedWithEnemy;
    void CheckRespawn(Transform lives)
	{
		if (lostLife || overExtended || collidedWithEnemy) {
			AudioSource.PlayClipAtPoint (wilhelm, player.transform.position);

            if(overExtended || collidedWithEnemy)
            {
                // Create Explosion
                // TODO
                // Figure out how to make it so that the explosions are on top of the other bases.
                Vector3 pos = player.transform.position;
                pos.y += 2.0f;
                //		pos.z += -10.0f;
                Quaternion angle = Quaternion.AngleAxis(0, Vector3.right);
                // Create a copy of the explosion 
                GameObject newExplosion = (GameObject)Instantiate(explosion, pos, angle);
                // Delete after 5 seconds
                Destroy(newExplosion, 5);

                //monster = GameObject.Find("Monster");
                //GameObject m = (GameObject)Instantiate(monster, pos,)

                Vector3 resetPos = new Vector3(0.0f, -12.05f, 10.06f);
                player.transform.position = resetPos;
                //player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(0.0f, 12.05f, 10.06f), 1);
            }

            // Destroy one by one
            foreach (Transform life in lives) {
				Destroy (life.gameObject);
				break;
			}

			StartCoroutine ("Respawn");
			lostLife = false;
			overExtended = false;
			collidedWithEnemy = false;
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
    // Horde Management
    // ---------------------------------------------------------------

    public bool hordeStart;
    void UpdateHordeTimer()
    {
        hordeTimer -= Time.deltaTime;
        foreach (Transform horder in hordeHolder.transform)
        {
            if (horder.tag == "HordeTimer")
            {
                horder.GetComponent<Text>().text = ": " + hordeTimer.ToString("N0");
            }
        }
    }

    // The enemies on the current level will move aside, making way
    // for the new horde attack. They will go towards the player 
    // faster than the normal enemies and will loop back into space
    // if they aren't destroyed. 
    // This is more of a mini game idea. 
    void StartHorde()
    {
        // This goes with the aesthetic 
        //enemies.transform.position += Vector3.down * 3.0f;

        //Vector3 pos = enemy10.transform.position;
        //pos.y -= 4;
        //Instantiate(enemy10, pos, enemy10.transform.rotation);

        //hordeTimer = 30;

        //				Vector3 pos = enemy10.transform.position;
        //				pos.y = 27;
        //				while (true) {
        //					enemy10.transform.position = Vector3.Lerp (enemy10.transform.position, pos, Time.deltaTime);
        //
        //					if (Mathf.Abs(enemy10.transform.position.y - pos.y) <= 0.75f) {
        //						break;
        //					}
        //				}

        // TODO
        // Add Winning Condition here
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

	IEnumerator RedDeadUFO()
	{
        // Tells PlayerController that the player can move all over the screen.
        invincible = true;

        // Start invincibility ------------------------------

        yield return new WaitForSeconds (5);

        // Give the player a warning that they have
        // (n/2) seconds left to return to the base.
        Debug.Log ("5 seconds to return to base");

        yield return new WaitForSeconds(5);

        // End invincibility ------------------------------

        // hitRedUFO will end the coroutine
        hitRedUFO = false;
        // hitRedUFO and invincible will stop
        // the user from moving all over the screen and
        // will be restricted back to horizontal movements.
        invincible = false;
        // This will allow the red ufo's to spawn again.
        // Will need to tweak this so they don't spawn 
        // right after invulnerability.
        spawnedRedUFO = false;

        invincibilityFinished = true;

        // Just to make sure we exit the coroutine immediately.
        yield break;
	}
}
