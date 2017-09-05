using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	private Transform player;
	public float speed;
	public float minBounds;
	public float maxBounds;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

	private float nextFire;

	// Use this for initialization
	void Start () 
	{
		player = GetComponent<Transform> ();	
	}
	
	void FixedUpdate () 
	{
		float h = Input.GetAxis ("Horizontal");

		if (player.position.x < minBounds && h < 0) {
			h = 0;
		} 
		else if (player.position.x > maxBounds && h > 0) {
			h = 0;
		}

		player.position += Vector3.right * h * speed;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;

			// TODO: FIX!
			// Need to see if there's a way to spawn bullet without it bouncing off the player.
			Vector3 pos = shotSpawn.position;
			pos.y += 1.5f;

			Instantiate (shot, pos, shotSpawn.rotation);
		}
	}
}
