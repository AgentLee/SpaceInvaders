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

	public bool freeToMove;

	public float EPSILON = 0.75f;

	// Use this for initialization
	void Start () 
	{
		g = GameObject.Find ("GlobalObject");

		player = GetComponent<Transform> ();	

		freeze = false;

		shotsFired = 0.0f;
		shotsHit = 0.0f;
		accuracy = 0.0f;

		startTime = 1.0f;

		reset = false;
		resetPosition = new Vector3 (0f, -12.05f, 10.06f);

		prepForLaunch = false;
		setToLaunch = false;

		freeToMove = false;
	}

	public bool invincible;
	public float maxSpeed = 25;
	public float acceleration;
	void FixedUpdate () 
	{
		if (!freeze) {
			MovePlayer ();
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

		// If the player gets hit by an enemy bullet,
		// we want to freeze all movement until the respawn is finished.
		freeze = g.GetComponent<Global> ().freeze;
		// If the player hits the Red UFO, 
		// the player is given (20) seconds to move around.
		// They will be given a (10) second warning to return to base.
		// If they don't then they basically get stuck where they are.
		freeToMove = g.GetComponent<Global> ().invincible;

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
	void MovePlayer()
	{
		if (g.GetComponent<Global> ().hitRedUFO && freeToMove) {
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

		TiltPlayer ();

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
