using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerController : MonoBehaviour 
{
	private Transform player;

	// Movement
	public float speed;
	public float tilt;
	public float minBounds;
	public float maxBounds;

	// Blaster properties
	public Transform shotSpawn;
	public GameObject shot;
	public float fireRate;
	private float nextFire;
	public AudioClip blaster;

	// Accuracy
	public bool hitEnemy;
	public float shotsFired;
	public float shotsHit;
	public float accuracy;

	// Global GameObject
	public GameObject g;
	public bool freeze;

	public float startTime;
	public Vector3 resetPosition;

	public bool prepForLaunch;
	public bool setToLaunch;
	public bool reset;

	public float EPSILON = 0.75f;

	public bool startGame;

	// Use this for initialization
	void Start () 
	{
		startGame = true;

		g = GameObject.Find ("GlobalObject");

		player = GetComponent<Transform> ();	

		freeze = false;

		shotsFired = 0.0f;
		shotsHit = 0.0f;
		accuracy = 0.0f;

		startTime = 1.0f;

		reset = false;
		resetPosition = new Vector3 (0f, -12.05f, 10.06f);

		invincible = false;
		prepForLaunch = false;
		setToLaunch = false;
	}

	public bool invincible;
	public float maxSpeed = 25;
	public float acceleration;
	void FixedUpdate () 
	{
		if (!freeze) {
			// Move player to the middle of the screen.
			// After doing this the player will be launched into battle
			// and can move all around the screen for about 10 seconds.
			if (prepForLaunch && !setToLaunch) {
				player.position = Vector3.Lerp (player.position, resetPosition, Time.deltaTime);

				if (Mathf.Abs (player.position.x - resetPosition.x) <= 0.5f) {
					prepForLaunch = false;
					setToLaunch = true;
				}

			} 
			else {
				MovePlayer ();
			}
		}

		if (shotsHit == 0.0f && shotsFired >= 0.0f) {
			accuracy = 0.0f;
		} else {
			accuracy = (shotsHit / shotsFired) * 100.0f;
		}
	}



	// Update is called once per frame
	void Update()
	{
//		if (g.GetComponent<Global> ().hitRedUFO) {
//			invulnerability -= Time.deltaTime;
//		}

		// If the player gets hit by an enemy bullet,
		// we want to freeze all movement until the respawn is finished.
		freeze = g.GetComponent<Global> ().freeze;

		if (!freeze) {
			if (Input.GetKeyDown (KeyCode.Space) || Input.GetButton ("Fire1")) {
				if (Time.time > nextFire) {
					nextFire = Time.time + fireRate;

					Vector3 pos = shotSpawn.position;
					pos.y += 1.5f;

					Instantiate (shot, pos, Quaternion.identity);

					AudioSource.PlayClipAtPoint (blaster, gameObject.transform.position);

					shotsFired++;
				}
			}
		}
	}

	// Moves player left/right
	// Want to extend this so that when the player hits the redUFO
	// they can move anywhere in space.
	public float invulnerability;
	void MovePlayer()
	{
		if (g.GetComponent<Global> ().hitRedUFO && g.GetComponent<Global>().invincible) {
			float v = Input.GetAxis ("Vertical");

			if (player.position.y < -12 && v < 0) {
				v = -12;
			} 
			else if (player.position.y > 25 && v > 0) {
				v = 25;
			}

			player.position += Vector3.up * v * speed;
		}

		float h = Input.GetAxis ("Horizontal");

		if (player.position.x < minBounds && h < 0) {
			h = 0;
		} 
		else if (player.position.x > maxBounds && h > 0) {
			h = 0;
		}

		// TODO
		// The player rotates on start about the x axis.
		if (!startGame) {
			TiltPlayer ();
		}
		else {
			startGame = false;
		}

		player.position += Vector3.right * h * speed;
	}


	float tiltAngle = -30.0f;
	void TiltPlayer()
	{
		float tiltZ = Input.GetAxis ("Horizontal") * tiltAngle;
		Quaternion angle = Quaternion.Euler (0, 0, tiltZ);
		player.rotation = Quaternion.Slerp (player.rotation, angle, Time.deltaTime * 2.0f);
	}

	IEnumerator Invulnerability()
	{
		float elapsed = 0;
		while(elapsed <= 3)
		{
			

			elapsed = Time.deltaTime;

			yield return null;
		}
	
		yield return null;
	}
}
