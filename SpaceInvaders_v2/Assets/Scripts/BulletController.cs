using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour 
{
	private Transform bullet;
	public float speed;

	// Use this for initialization
	void Start () 
	{
		bullet = GetComponent<Transform> ();	
	}

	void FixedUpdate()
	{
		bullet.position += Vector3.up * speed;

		// Check
		if (bullet.position.y >= 11) {
			Destroy (gameObject);
		}
	}

	public GameObject deathExplosion;
	void OnTriggerEnter(Collider collider)
	{
		Instantiate (deathExplosion, gameObject.transform.position, Quaternion.AngleAxis (0, Vector3.right));

		if (collider.tag == "Enemy") {
			Destroy (collider.gameObject);
			Destroy (gameObject);

			// TODO
			// Increase score
		}
		else if (collider.tag == "Base") {
			Destroy (collider.gameObject);
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
