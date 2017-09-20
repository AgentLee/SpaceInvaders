using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	public GameObject pausedText;
    public GameObject resumeButton;
    public GameObject quitButton;
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

    public float resourceTimer;

    public GameObject enemiesDestroyedText;
    public GameObject enemiesBreachedText;

    public GameObject engine;

    public bool resource_life;
    public bool resource_shields;

    IEnumerator SafeZone()
    {
        yield return new WaitForSeconds(5);

        safeZoneText.GetComponent<Text>().enabled = false;

        yield break;
    }

    // Use this for initialization
    void Start ()
	{
        safeZoneText = GameObject.Find("SafeZoneText");
        safeZoneText.GetComponent<Text>().enabled = true;
        StartCoroutine("SafeZone");

        resource_life = false;
        resource_shields = false;

        hyperspace = GameObject.Find("Hyperspace").GetComponent<ParticleSystem>();
        hyperspace.Stop();
        engine = GameObject.Find("Engine");
        
        // Hide the cursor
		Cursor.visible = false;

		invincible = false;

		// UI
		scores = new int[5];
		score = 0;
		timer = 0;

        resourceTimer = Random.Range(20, 40);

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

        // Horde Timer
        hordeTimer = 30;
        hordeHolder = GameObject.Find("Horde").gameObject;
        foreach(Transform horder in hordeHolder.transform)
        {
            if(horder.tag == "HordeTimer")
            {
                horder.GetComponent<Text>().text = ": " + hordeTimer.ToString("N0");
            }
        }

        // Pause Text
        pausedText = GameObject.Find("Pause").gameObject;
        pausedText.GetComponent<Text>().enabled = false;

        // Horde Stats
        enemiesDestroyedText = GameObject.Find("DestroyedEnemies");
        enemiesDestroyedText.GetComponent<Text>().enabled = false;

        enemiesBreachedText = GameObject.Find("EnemiesBreached");
        enemiesBreachedText.GetComponent<Text>().enabled = false;

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

        SpawnEnemies(0);
        spawnedEnemies = false;

        resumeButton = GameObject.Find("Resume").gameObject;
        resumeButton.GetComponent<Text>().enabled = false;

        quitButton = GameObject.Find("Quit").gameObject;
        quitButton.GetComponent<Text>().enabled = false;

        enemiesBreached = 0;

        counter = Random.Range(20, 40);

        status = GameObject.Find("Status");
        status.GetComponent<Text>().text = "Attack!!";

        shieldTimer = 3;
    }

    public int counter;
    public AudioClip lightspeed;

    public ParticleSystem hyperspace;
    public bool spawnedEnemies;
    public int enemiesBreached;
    void SpawnEnemies(int level)
    {
        Vector3 enemy10pos = enemy10.transform.position;
        Vector3 enemy20pos = enemy20.transform.position;
        Vector3 enemy30pos = enemy30.transform.position;

        enemy10pos.x += level * 5.0f;
        enemy20pos.x += level * 5.0f;
        enemy30pos.x += level * 5.0f;

        enemy10Group = Instantiate(enemy10, enemy10pos, enemy10.transform.rotation);
        enemy20Group = Instantiate(enemy20, enemy20pos, enemy20.transform.rotation);
        enemy30Group = Instantiate(enemy30, enemy30pos, enemy30.transform.rotation);

        spawnedEnemies = true;

        paused = false;
    }

    public GameObject enemy10Group;
    public GameObject enemy20Group;
    public GameObject enemy30Group;

    public bool paused;
    public AudioClip barrierDestroyed;
    public bool playedBarrierDestroyedClip;

    public IEnumerator ShowHordeStats()
    {
        enemiesBreachedText.GetComponent<Text>().text = enemiesBreached.ToString() + " enemies breached";
        enemiesBreachedText.GetComponent<Text>().enabled = true;

        yield return new WaitForSeconds(5);

        enemiesBreached = 0;
        enemiesBreachedText.GetComponent<Text>().enabled = false;

        endedHordeAttack = false;

        yield break;
    }

    public AudioClip gotShieldBlip;
    public AudioClip shieldGen;
    public bool endedHordeAttack;

    public int enemiesHordeDestroyed;

    public float shieldTimer;

    // Update is called once per frame
    void Update () 
	{
        if(resource_shields)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                shieldTimer = 3;
                resource_shields = false;
            }
        }
        

        timer += Time.deltaTime;

        if(endedHordeAttack)
        {
            StartCoroutine("ShowHordeStats");
        }
        if(!endedHordeAttack)
        {
            StopCoroutine("ShowHordeStats");
        }

        UpdateHordeTimer();
        if(hordeTimer <= 0.0f)
        {
            hordeStart = true;
            if (!playedBarrierDestroyedClip)
            {
                AudioSource.PlayClipAtPoint(barrierDestroyed, this.transform.position);
                playedBarrierDestroyedClip = true;
            }

            foreach (Transform horder in hordeHolder.transform)
            {
                if (horder.tag == "HordeTimer")
                {
                    horder.GetComponent<Text>().text = "HORDE INCOMING!";
                }
            }
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

        // Shows/hides lives
        LifeCount(livesObject.transform);
		// Rotate the lives at the bottom left
		RotateLives (livesObject.transform);
		// Check to see if the player died and needs to respawn
		CheckRespawn (livesObject.transform);

		// TODO
		CheckLevelUp();

		UpdateHighScore ();

        // Player holds tab to see accuracy and base health
		accuracy.text = "Accuracy: " + player.gameObject.GetComponent<PlayerController> ().accuracy.ToString ("F2") + "%";
		if (Input.GetKey (KeyCode.Tab)) {
			accuracy.enabled = true;
		} else {
			accuracy.enabled = false;
		}

        // Player presses Escape to pause the game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ActivatePauseMenu();
        }

		if (hitEnemy)
        {
			hitEnemy = false;

			player.gameObject.GetComponent<PlayerController> ().shotsHit++;
		}
	
		if (hitRedUFO) {
            StartCoroutine ("RedDeadUFO");
		}
        else if(!hitRedUFO)
        {
            StopCoroutine("RedDeadUFO");
        }

        // See if there's another way without having a new boolean.
        if(invincibilityFinished)
        {
            MovePlayerToSafeZone();
        }

        resourceTimer -= Time.deltaTime;
        if(resourceTimer <= 0 && !spawnResources)
        {
            spawnResources = true;
            resourceTimer = Random.Range(20, 40);
        }

        if(resource_life)
        {
            AudioSource.PlayClipAtPoint(oneup, new Vector3(0, 0,0));

            if (numLives == 3)
            {
                score += 50;
            }
            else
            {
                numLives++;
            }

            resource_life = false;
        }

        // Restart Level
        if(Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("Level001");
        }

        status = GameObject.Find("Status");
        status.GetComponent<Text>().text = "ATTACK!";
    }

    public GameObject status;
    public AudioClip oneup;

    void UpdateResourceTimer()
    {
        resourceTimer += Time.deltaTime;
    }

    void MovePlayerToSafeZone()
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
            if (Vector3.Distance(playerPos, startPos) > 0.5f)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, startPos, Time.deltaTime);
                invincibilityFinished = true;
            }
        }
        else
        {
            overExtended = true;
        }
    }

    public GameObject safeZoneText;

    // ---------------------------------------------------------------
    // Pause Menu
    // ---------------------------------------------------------------

    void ActivatePauseMenu()
    {
        paused = !paused;

        if (!paused)
        {
            Cursor.visible = false;

            pausedText.GetComponent<Text>().enabled = false;

            resumeButton.SetActive(false);
            quitButton.SetActive(false);

            Time.timeScale = 1;
        }
        else
        {
            Cursor.visible = true;

            pausedText.GetComponent<Text>().enabled = true;

            resumeButton.SetActive(true);
            quitButton.SetActive(true);

            Time.timeScale = 0;
        }
    }

    public void PauseMenu(int option)
    {
        if (option == 0)
        {
            resumeButton.SetActive(false);
            quitButton.SetActive(false);

            paused = false;
            pausedText.GetComponent<Text>().enabled = false;
            Time.timeScale = 1;
        }
        else if(option == 1)
        {
            resumeButton.SetActive(false);
            quitButton.SetActive(false);

            Debug.Log("Quit");
            Application.Quit();
        }
    }

    // ---------------------------------------------------------------
    // End Game Conditions
    // ---------------------------------------------------------------

    bool EnemiesReachedBase()
    {
        if (enemy10Group.transform.position.y <= -4 || enemy20Group.transform.position.y <= -4 || enemy30Group.transform.position.y <= -4)
        {
            return true;
        }

        return false;
	}

	bool RanOutOfLives()
	{
		if (lostLife && numLives == 0) {
			AudioSource.PlayClipAtPoint (wilhelm, player.transform.position);

			foreach (Transform life in livesObject.transform) {
                MeshRenderer render = life.gameObject.GetComponent<MeshRenderer>();
                render.enabled = false;
                //Destroy(life.gameObject);
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

    void LifeCount(Transform lives)
    {
        if(resource_life)
        {
            if(numLives == 3)
            {

            }
            else if(numLives == 2)
            {

            }
            else if(numLives == 1)
            {

            }
        }

        Debug.Log(numLives);

        int count = 0;
        foreach (Transform life in lives)
        {
            MeshRenderer render = life.gameObject.GetComponentInChildren<MeshRenderer>();
            count++;
            if (count <= numLives)
            {
                render.enabled = true;
            }
            else
            {
                render.enabled = false;
            }
        }
    }

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

                numLives--;
                //player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(0.0f, 12.05f, 10.06f), 1);
            }

            // Destroy one by one
            int count = 0;
            foreach (Transform life in lives) {
                count++;

                MeshRenderer render = life.gameObject.GetComponentInChildren<MeshRenderer>();
                if (count == numLives)
                    render.enabled = false;
                    //Destroy (life.gameObject);
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
	}

    public bool showedLevelUp;
    IEnumerator showLevelUp()
    {

        yield return new WaitForSeconds(5);

        showedLevelUp = true;

        yield break;
    }

    public int level;
	void CheckLevelUp()
	{
		// Level Up
		if (numEnemies <= 0) {
          //  if(!showedLevelUp)
          //  {
		        //levelUp.GetComponent<Text> ().enabled = true;
          //      StartCoroutine("showLevelUp");
          //  } 
          //  else if(showedLevelUp)
          //  {
          //      StopCoroutine("showLevelUp");
		        //levelUp.GetComponent<Text> ().enabled = false;
          //      showedLevelUp = false;
          //  }

            numLives++;

            if(!spawnedEnemies)
                SpawnEnemies(++level);
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
        engine.GetComponent<ParticleRenderer>().enabled = false;

        // Respawn time
        yield return new WaitForSeconds (1);

		// Reset player rotation
		player.transform.rotation = Quaternion.identity;

		// Flash the player
		float timeToBlink = Time.time + 3;
		while (Time.time < timeToBlink) {
			yield return new WaitForSeconds (0.5f);
			 
			render.enabled = !render.enabled;
            engine.GetComponent<ParticleRenderer>().enabled = !engine.GetComponent<ParticleRenderer>().enabled;
        }

        // Just to make sure the player is shown
        render.enabled = true;
        engine.GetComponent<ParticleRenderer>().enabled = true;

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

        ActivatePauseMenu();

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
        status = GameObject.Find("Status");
        status.GetComponent<Text>().text = "RETURN TO BASE!";

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
