﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour 
{
	public GameObject explosion;
	private Transform bullet;
	public float speed;

	// Use this for initialization
	void Start () 
	{
		bullet = GetComponent<Transform> ();	
	}

	void FixedUpdate()
	{
		bullet.position -= Vector3.up * speed;

		// Check
		if (bullet.position.y <= -20) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Line") {
			return;
		}

		if (collider.tag == "Enemy") {
			return;
		}

		// TODO
		// Figure out how to make it so that the explosions are on top of the other bases.
		Vector3 pos = gameObject.transform.position;
		pos.y -= 2.0f;
		Quaternion angle = Quaternion.AngleAxis (0, Vector3.right);
		// Create a copy of the explosion 
		GameObject newExplosion = (GameObject)Instantiate (explosion, pos, angle);
		// Delete after 5 seconds
		Destroy (newExplosion, 5);

		if (collider.tag == "Base") {
			Destroy (collider.gameObject);
			Destroy (gameObject);

			GameObject obj = GameObject.Find ("GlobalObject");
			Global g = obj.GetComponent<Global> ();
			g.baseCount--;
		} 
		else if (collider.tag == "Player") {
			//Destroy (collider.gameObject);
			Destroy (gameObject);

			GameObject obj = GameObject.Find ("GlobalObject");
			Global g = obj.GetComponent<Global> ();
			g.numLives--;
			g.lostLife = true;
		} 
	}

	// Update is called once per frame
	void Update () 
	{

	}
}
