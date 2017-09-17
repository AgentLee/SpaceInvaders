using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour 
{
	public GameObject explosion;
	private Transform bullet;
	public float speed;

	public AudioClip bulletSound;

	public GameObject g;

	// Use this for initialization
	void Start () 
	{
		g = GameObject.Find ("GlobalObject");

		bullet = GetComponent<Transform> ();	
	}

	void FixedUpdate()
	{
		bullet.position -= Vector3.up * speed;

		// Destroy the bullet if it goes out of bounds
		if (bullet.position.y <= -20) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		// Checks to see if the bullet hit
		// other bullets, the line element, or other enemies.
		// If it does then an explosion shouldn't occur and 
		// we just return out of the function.
		if (!CheckBulletCollision (collider.tag)) {
			return;
		}

		switch (collider.tag) 
		{
			case "Base":
				// Destroy base piece
				Destroy (collider.gameObject);
				g.GetComponent<Global> ().baseCount--;
				break;
			case "Player":
				g.GetComponent<Global> ().numLives--;
				// Trigger Respawn() in Global
				g.GetComponent<Global> ().lostLife = true;
				break;
			default:
				break;
		}

		AudioSource.PlayClipAtPoint (bulletSound, gameObject.transform.position);

		// Create an explosion
		// TODO
		// Figure out how to make it so that the explosions are on top of the other bases.
		Vector3 pos = gameObject.transform.position;
		pos.y -= 2.0f;
		Quaternion angle = Quaternion.AngleAxis (0, Vector3.right);
		// Create a copy of the explosion 
		GameObject newExplosion = (GameObject)Instantiate (explosion, pos, angle);
		// Delete after 5 seconds
		Destroy (newExplosion, 5);

		// Destroy the bullet
		Destroy (gameObject);
	}

	bool CheckBulletCollision(string tag)
	{
		switch (tag) 
		{
			case "Line":
				return false;
			case "Enemy":
				return false;
			// Shouldn't ever get to this but
			// BKR taught me about defensive programming. 
			case "RedUFO":
				return false;
			case "Bullet":
				return false;
			case "EnemyBullet":
				return false;
			default:
				return true;
		}
	}
}
