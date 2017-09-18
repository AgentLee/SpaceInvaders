using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerController : MonoBehaviour 
{
	private Transform player;

	// Movement
	public float speed;
	public float tiltAngle;
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

		reset = false;

		prepForLaunch = false;
		setToLaunch = false;

		freeToMove = false;

        tiltAngle = -30.0f;
	}

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

	// Moves player left/right unless the RedUFO is hit.
	void MovePlayer()
	{
        // If the RedUFO is hit then the player can move all around.
        // They have (10) seconds to do whatever. At (5) seconds they'll get
        // a warning to return to the safe zone.
		if (g.GetComponent<Global> ().hitRedUFO && freeToMove) {
			float v = Input.GetAxis ("Vertical");

			if (player.position.y < -15 && v < 0 || 
                player.position.y > 15 && v > 0)
            {
				v = 0;
			}

			player.position += Vector3.up * v * speed;
		}

		float h = Input.GetAxis ("Horizontal");

		if (player.position.x < minBounds && h < 0 ||
            player.position.x > maxBounds && h > 0)
        {
			h = 0;
		}

        // Control player rotation when moving left/right
		TiltPlayer ();
		player.position += Vector3.right * h * speed;
	}

	void TiltPlayer()
	{
		float tiltZ = Input.GetAxis ("Horizontal") * tiltAngle;
		Quaternion angle = Quaternion.Euler (0, 0, tiltZ);
		player.rotation = Quaternion.Slerp (player.rotation, angle, Time.deltaTime * 2.0f);
	}

    // If the player flies into an enemy ship, 
    // it gets destroyed. This is handled in the
    // Global script.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            g.GetComponent<Global>().collidedWithEnemy = true;
        }

        // TODO
        // Add base to this
    }
}
