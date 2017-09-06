using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour 
{
	public float timer;
	public Vector3 originInScreenCoords;
	public int score;

	public int numEnemies;

	public GameObject player;
	public bool lostLife;
	public int numLives;

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

		originInScreenCoords = Camera.main.WorldToScreenPoint (new Vector3 (0, 0, 0));
	}

	void FixedUpdate()
	{
		
	}

	public Vector3 rotateAmount;

	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		// Level Up
		if (numEnemies == 0) {

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
				Debug.Log ("YOU LOSE");

				Time.timeScale = 0;
			}
			else {
				lostLife = false;
			}
		}



		if (numLives > 0 && lostLife) {

			lostLife = false;

			//Respawn ();
		}
	}

	IEnumerator Respawn()
	{
		GameObject tempPlayer = player.gameObject;

		MeshRenderer render = player.gameObject.GetComponentInChildren<MeshRenderer> ();
		render.enabled = false;

		
		yield return new WaitForSeconds(10);

		//render.enabled = true;
	}
}
