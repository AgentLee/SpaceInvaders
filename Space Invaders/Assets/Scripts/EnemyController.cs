using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public bool shouldBeShooting;

	public bool raid;

	public AudioClip blasterSound;

    public GameObject g;

	// Use this for initialization
	void Start () 
	{
		InvokeRepeating ("MoveEnemy", 0.1f, 0.3f);

		enemies = GetComponent<Transform> ();

		freeze = false;

		raid = false;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Bullet") {
			//Debug.Log (pointValue);
		}
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

	public bool freeze;
	void FixedUpdate()
	{
		g = GameObject.Find ("GlobalObject");

        freeze = g.GetComponent<Global> ().freeze; 
	}

	void Update()
	{
		if (raid) {
			foreach (Transform enemy in enemies) {
				enemy.position += Vector3.down * 2.0f;
			}
		}
	}


	void MoveEnemy()
	{
		if(!freeze) {
			enemies.position += Vector3.right * speed;

			foreach (Transform enemy in enemies) {
				if (enemy.position.x < -15 || enemy.position.x > 15) {
					// Have them move left to right, right to left
					speed = -speed;
					// Move them down
					enemies.position += Vector3.down * 0.5f;

					return;
				}

				// TODO
				// FIX THIS
				if (Random.value > fireRate && shouldBeShooting) {
					AudioSource.PlayClipAtPoint (blasterSound, gameObject.transform.position);
					Vector3 pos = this.transform.position;
					pos.y -= 1.0f;
					Instantiate (shot, pos, this.transform.rotation);
				}
			}
		}
	}
}
