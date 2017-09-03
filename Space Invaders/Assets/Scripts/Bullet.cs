using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
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

		if (bullet.position.y >= 10) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		// Destroy bullet and enemy
		if (collider.tag == "Enemy") {
			Destroy (collider.gameObject);
			Destroy (gameObject);

			// TODO
			// Increase player score
		}
		// Only destroy the bullet 
		else if (collider.tag == "Base") {
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
