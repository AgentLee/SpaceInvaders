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

	public bool reset;

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
	}

	void FixedUpdate () 
	{
		if (!freeze) {
			if (reset) {
				if (player.position.y > resetPosition.y || player.position.y < resetPosition.y) {
					Vector3 moveDirection = (resetPosition - player.position).normalized;

					player.position += moveDirection * speed * Time.deltaTime;
					player.rotation = Quaternion.identity;
				}
				else {
					reset = false;
				}

//				player.position = Vector3.Lerp (player.position, resetPosition, .01f);
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

					Instantiate (shot, pos, shotSpawn.rotation);

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

		//TiltPlayer();

		player.position += Vector3.right * h * speed;
	}

	// TODO
	void TiltPlayer()
	{
		if (Input.anyKey) {
			if (Input.GetKey(KeyCode.A)) {
				player.transform.rotation = Quaternion.Euler (0.0f, 0.0f, (transform.position).x * -tilt);
			} else if (Input.GetKey (KeyCode.D)) {
				player.transform.rotation = Quaternion.Euler (0.0f, 0.0f, (transform.position).x * -tilt);
			}
		}
		else {
			player.transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0 * -tilt);
		}
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
