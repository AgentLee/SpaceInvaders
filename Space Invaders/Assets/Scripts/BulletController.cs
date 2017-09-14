using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour 
{
	public GameObject explosion;
	private Transform bullet;
	public float speed;
	public AudioClip blaster;

	public GameObject g;

	// Use this for initialization
	void Start () 
	{
		bullet = GetComponent<Transform> ();	

		g = GameObject.Find ("GlobalObject");
	}

	void FixedUpdate()
	{
		bullet.position += Vector3.up * speed;

		// Destroy the bullet if it goes out of bounds
		if (bullet.position.y >= 20) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		AudioSource.PlayClipAtPoint (blaster, gameObject.transform.position);

		// Create Explosion
		// TODO
		// Figure out how to make it so that the explosions are on top of the other bases.
		Vector3 pos = gameObject.transform.position;
		pos.y += 2.0f;
		//		pos.z += -10.0f;
		Quaternion angle = Quaternion.AngleAxis (0, Vector3.right);
		// Create a copy of the explosion 
		GameObject newExplosion = (GameObject)Instantiate (explosion, pos, angle);
		// Delete after 5 seconds
		Destroy (newExplosion, 5);

		// Destroy the bullet
		Destroy (gameObject);
		// Destroy the enemy/base piece/red ufo
		Destroy (collider.gameObject);

		if (collider.tag == "Enemy") {
			EnemyController enemy = collider.gameObject.GetComponent<EnemyController> ();

			// Update score and enemy counter
			g.GetComponent<Global> ().score += enemy.pointValue;
			g.GetComponent<Global> ().numEnemies--;

			// Update player's accuracy
			g.gameObject.GetComponent<Global>().hitEnemy = true;
		} 
		else if (collider.tag == "Base") {
			g.GetComponent<Global> ().baseCount--;
		} 
		else if (collider.tag == "RedUFO") {
			RedUFOController ufo = collider.gameObject.GetComponent<RedUFOController> ();

			ufo.hit = true;

			// Get random point value in the point values array
			int pointValue = ufo.pointValues[Random.Range (0, 3)];
			g.GetComponent<Global> ().score += pointValue;

			g.GetComponent<Global> ().hitRedUFO = true;

			// Update player's accuracy
			PlayerController player = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
			player.shotsHit++;

			Global global = g.gameObject.GetComponent<Global> ();
			Debug.Log (global.player.gameObject.GetComponent<PlayerController> ().shotsHit);
		}
	}
}
