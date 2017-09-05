using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour 
{
	private Transform enemyHolder;
	public float speed;
	public GameObject shot;
	public Text winText;
	public float fireRate = 0.997f;

	// Use this for initialization
	void Start () 
	{
		winText.enabled = false;

		InvokeRepeating ("MoveEnemy", 0.1f, 0.3f);

		enemyHolder = GetComponent<Transform> ();
	}

	void MoveEnemy()
	{
		enemyHolder.position += Vector3.right * speed;

		foreach (Transform enemy in enemyHolder) {
			if (enemy.position.x < -10.5 || enemy.position.x > 10.5) {
				// Have them move left to right, right to left
				speed = -speed;
				// Move them down
				enemyHolder.position += Vector3.down * 0.5f;

				return;
			}

			if (Random.value > fireRate) {
				Instantiate (shot, enemy.position, enemy.rotation);
			}
		
			// Enemy reached the bases
			if (enemy.position.y <= -4) {
				GameOver.isPlayerDead = true;

				Time.timeScale = 0;
			}
		}

		if (enemyHolder.childCount == 1) {
			CancelInvoke ();
			InvokeRepeating ("MoveEnemy", 0.1f, 0.25f);
		}

		if (enemyHolder.childCount == 0) {
			winText.enabled = true;
		}
	}
}
