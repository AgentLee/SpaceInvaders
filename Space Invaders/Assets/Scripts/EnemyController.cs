using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base enemy class
// There are 3 types of enemies that are made as prefabs
// and are given 10, 20, 30 pointValues.
public class EnemyController : MonoBehaviour
{
	public int pointValue;

	private Transform enemies;
	public float speed;
	public GameObject shot;
	public float fireRate = 0.95f;

	// Use this for initialization
	void Start () 
	{
		InvokeRepeating ("MoveEnemy", 0.1f, 0.3f);

		enemies = GetComponent<Transform> ();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Bullet") {
			//Debug.Log (pointValue);
		}
	}

	void MoveEnemy()
	{
		enemies.position += Vector3.right * speed;

		foreach (Transform enemy in enemies) {
			if (enemy.position.x < -15|| enemy.position.x > 15) {
				// Have them move left to right, right to left
				speed = -speed;
				// Move them down
				enemies.position += Vector3.down * 0.5f;

				return;
			}

			if (Random.value > fireRate) {
				Vector3 pos = enemy.position;
				pos.y -= 1.0f;
				Instantiate (shot, pos, enemy.rotation);
			}

			// Enemy reached the bases
			if (enemy.position.y <= -4) {
				//GameOver.isPlayerDead = true;

				//Time.timeScale = 0;
			}
		}
	}
}
