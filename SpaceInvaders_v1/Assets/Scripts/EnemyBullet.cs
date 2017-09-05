using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour 
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
		bullet.position += Vector3.up * -speed;

		// Handle offscreen
		if (bullet.position.y <= -10) {
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			Destroy (collider.gameObject);
			Destroy (gameObject);

			GameOver.isPlayerDead = true;
		}
		else if (collider.tag == "Base") {
			GameObject playerBase = collider.gameObject;

			Base baseHealth = playerBase.GetComponent<Base> ();
			baseHealth.health -= 1;

			Destroy (gameObject);
		}
	}
}
