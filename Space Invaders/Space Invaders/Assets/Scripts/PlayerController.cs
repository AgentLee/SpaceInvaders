using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	private Transform player;
	public float speed;
	public float maxBounds;
	public float minBounds;

	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate;

	private float nextFire;

	// Use this for initialization
	void Start () {
		player = GetComponent<Transform> ();	
	}

	void FixedUpdate() 
	{
		float h = Input.GetAxis ("Horizontal");

		// Stops player from going off screen
		if (player.position.x < minBounds && h < 0) {
			h = 0;
		} 
		else if (player.position.x > maxBounds && h > 0) {
			h = 0;
		}

		player.position += Vector3.right * h * speed;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1") || Input.GetKeyDown(KeyCode.Space)) {
			if (Time.time > nextFire) {
				// Determines when the player is allowed to shoot another bullet.
				nextFire = Time.time + fireRate;

				// Create the bullet
				Instantiate (bullet, bulletSpawn.position, bulletSpawn.rotation);
			}
		}
	}
}
