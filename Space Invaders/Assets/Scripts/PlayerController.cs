using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	private Transform player;
	public float speed;
	public float tilt;
	public float minBounds;
	public float maxBounds;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

	private float nextFire;

	public AudioClip blaster;

	public GameObject g;
	public bool freeze;

	// Use this for initialization
	void Start () 
	{
		g = GameObject.Find ("GlobalObject");

		player = GetComponent<Transform> ();	

		freeze = false;
	}
	
	void FixedUpdate () 
	{
		if (!freeze) {
			MovePlayer ();
		}
	}

	void MovePlayer()
	{
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

	// Update is called once per frame
	void Update()
	{
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
				}
			}
		}
	}
}
