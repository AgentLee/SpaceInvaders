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

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Enemy") {
			Destroy (collider.gameObject);
			Destroy (gameObject);

			// TODO
			// Increase score
		}
		// TODO
		else if (collider.tag == "Base") {
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
