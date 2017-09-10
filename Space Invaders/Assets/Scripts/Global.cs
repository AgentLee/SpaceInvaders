using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour 
{
	public float timer;
	public Vector3 originInScreenCoords;
	public int score;

	public GameObject enemies;
	public int numEnemies;

	public GameObject player;
	public bool lostLife;
	public bool freezeEnemies;
	public int numLives;

	// TODO
	// Find a better way to delete bases
	public int baseCount;

	// Use this for initialization
	void Start ()
	{
		score = 0;
		timer = 0;

		// TODO
		// Find a better way to keep track of enemies
		numEnemies = 3;

		lostLife = false;
		numLives = 3;

		freezeEnemies = false;

		originInScreenCoords = Camera.main.WorldToScreenPoint (new Vector3 (0, 0, 0));

		GameObject g = GameObject.Find ("GameOver").gameObject;
		g.GetComponent<Text> ().enabled = false;

		baseCount = 40;
	}

	void FixedUpdate()
	{
		
	}

	public Vector3 rotateAmount;

	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		GameObject bases = GameObject.Find ("Bases").gameObject;

		if (baseCount == 0) {
			StartCoroutine ("GG");

			GameObject g = GameObject.Find ("GameOver").gameObject;
			g.GetComponent<Text> ().enabled = true;
		}

		if (enemies.transform.position.y <= 1.0f) {
			StartCoroutine ("GG");

			GameObject g = GameObject.Find ("GameOver").gameObject;
			g.GetComponent<Text> ().enabled = true;
		}

		// Level Up
		if (numEnemies <= 0) {
			Debug .Log("BASES GONE");
		}

		GameObject obj = GameObject.Find ("Lives");
		Transform lives = obj.transform;
		foreach (Transform life in lives) {
			life.Rotate (rotateAmount * Time.deltaTime);
		}

		if (lostLife) {
			// Destroy one by one
			foreach (Transform life in lives) {
				Destroy (life.gameObject);
				break;
			}

			if (numLives == 0) {
				StartCoroutine ("GG");

				GameObject g = GameObject.Find ("GameOver").gameObject;
				g.GetComponent<Text> ().enabled = true;
			}
			else {
				StartCoroutine ("Respawn");
				lostLife = false;
			}
		}
	}

	IEnumerator Respawn()
	{
		freezeEnemies = true;
		
		MeshRenderer render = player.gameObject.GetComponentInChildren<MeshRenderer> ();
		render.enabled = false;

		yield return new WaitForSeconds (1);

		// Flash
		float timeToBlink = Time.time + 3;
		while (Time.time < timeToBlink) {
			yield return new WaitForSeconds (0.5f);
			 
			render.enabled = !render.enabled;
		}

		freezeEnemies = false;

		render.enabled = true;
	}

	IEnumerator GG()
	{
		DestroyObject (player);

		yield return new WaitForSeconds (10);

		Time.timeScale = 0;
	}
}
