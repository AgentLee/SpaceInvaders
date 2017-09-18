using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class handles a group of enemies.
 */
public class NewEnemyScript : MonoBehaviour
{
    private Transform enemies;
    public float speed;
    public GameObject shot;
    public float fireRate = 0.95f;

    public bool shouldBeShooting;

	public AudioClip blasterSound;
    public bool partOfHorde;

    public GameObject g;

    public bool freeze;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("MoveEnemy", 0.1f, 0.3f);

        g = GameObject.Find("GlobalObject");

        enemies = GetComponent<Transform>();

        freeze = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void MoveEnemy()
    {
        freeze = g.GetComponent<Global> ().freeze;

        if (!partOfHorde && !freeze)
        {
            enemies.position += Vector3.right * speed;

            if (enemies.position.x < -15 || enemies.position.x > 15)
            {
                // Have them move left to right, right to left
                speed = -speed;
                // Move them down
                enemies.position += Vector3.down * 0.5f;

                return;
            }
        }
        else {

        }

        if(shouldBeShooting)
        {
            foreach (Transform enemy in enemies)
            {
                if (Random.value > fireRate && shouldBeShooting)
                {
                    AudioSource.PlayClipAtPoint(blasterSound, gameObject.transform.position);
                    Vector3 pos = enemy.transform.position;
                    pos.y -= 1.0f;
                    Instantiate(shot, pos, this.transform.rotation);
                }
            }
        }
    }
}