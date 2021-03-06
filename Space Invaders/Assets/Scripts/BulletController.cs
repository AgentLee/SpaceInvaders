﻿using System.Collections;
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

        shieldTimer = 3;
    }

    public float shieldTimer;
	void FixedUpdate()
	{
		bullet.position += Vector3.up * speed;

		// Destroy the bullet if it goes out of bounds
		if (bullet.position.y >= 20) {
			Destroy (gameObject);
		}

        if(startShieldTimer)
        {
            shieldTimer -= Time.deltaTime;

            if(shieldTimer <= 0.0f)
            {
                startShieldTimer = false;
                shieldTimer = 3;
            }
        }
	}

    public bool startShieldTimer;
    //public bool shields;
    //IEnumerator ShieldsUp()
    //{
    //    AudioSource.PlayClipAtPoint(g.GetComponent<Global>().shieldGen, new Vector3(0, 0, 0));
    //    yield return new WaitForSeconds(3);
    //    shields = false;
    //    yield break;
    //}

    void OnTriggerEnter(Collider collider)
	{
        if (!g.GetComponent<Global>().hordeStart && g.GetComponent<Global>().resource_shields && collider.tag == "Base")
        {
            startShieldTimer = true;
            //if(shields)
            //    StartCoroutine("ShieldsUp");
            //else if(!shields)
            //{
            //    StopCoroutine("ShieldsUp");
            //}

            return;
        }

        if(g.GetComponent<Global>().shieldTimer > 0)
        {
            Debug.Log("SHIELDS UP");
        } else
        {
            Debug.Log("SHIELDS DOWN");
        }
       

        if(!g.GetComponent<Global>().hordeStart)
        {
        AudioSource.PlayClipAtPoint (blaster, gameObject.transform.position);
            // Create Explosion
            // TODO
            // Figure out how to make it so that the explosions are on top of the other bases.
            Vector3 pos = gameObject.transform.position;
            pos.y += 2.0f;
            //pos.z += -5.0f;
            Quaternion angle = Quaternion.AngleAxis(0, Vector3.right);
            // Create a copy of the explosion 
            GameObject newExplosion = (GameObject)Instantiate(explosion, pos, angle);
            // Delete after 5 seconds
            Destroy(newExplosion, 5);

            // Destroy the bullet
            Destroy(gameObject);
            // Destroy the enemy/base piece/red ufo
            Destroy(collider.gameObject);
        }

        // Freeze all other enemies during the horde
        if(collider.tag == "Horder" && g.GetComponent<Global>().hordeStart)
        {
        AudioSource.PlayClipAtPoint (blaster, gameObject.transform.position);
            // Create Explosion
            // TODO
            // Figure out how to make it so that the explosions are on top of the other bases.
            Vector3 pos = gameObject.transform.position;
            pos.y += 2.0f;
            pos.z += -5.0f;
            Quaternion angle = Quaternion.AngleAxis(0, Vector3.right);
            // Create a copy of the explosion 
            GameObject newExplosion = (GameObject)Instantiate(explosion, pos, angle);
            // Delete after 5 seconds
            Destroy(newExplosion, 5);

            // Destroy the bullet
            Destroy(gameObject);
            // Destroy the enemy/base piece/red ufo
            Destroy(collider.gameObject);
        }

		if (collider.tag == "Enemy" && !g.GetComponent<Global>().hordeStart) {
			EnemyController enemy = collider.gameObject.GetComponent<EnemyController> ();

            //NewEnemyScript e = collider.gameObject.GetComponent<NewEnemyScript>();

            NewEnemyControllerScript e = collider.gameObject.GetComponent<NewEnemyControllerScript>();

			// Update score and enemy counter
			//g.GetComponent<Global> ().score += enemy.pointValue;
			g.GetComponent<Global> ().score += e.pointValue;
			g.GetComponent<Global> ().numEnemies--;

			// Update player's accuracy
			g.gameObject.GetComponent<Global>().hitEnemy = true;
		} 
		else if (collider.tag == "Base" && !g.GetComponent<Global>().hordeStart) {
			g.GetComponent<Global> ().baseCount--;
		} 
		else if (collider.tag == "RedUFO" && !g.GetComponent<Global>().hordeStart) {
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

        Debug.Log(collider.tag);

        if (collider.tag == "Resource1")
        {
            Debug.Log("EXTRA LIFE");
            g.GetComponent<Global>().resource_life = true;

            AudioSource.PlayClipAtPoint(g.GetComponent<Global>().oneup, new Vector3(0, 0, 0));

            // Destroy the bullet
            Destroy(gameObject);
            // Destroy the enemy/base piece/red ufo
            Destroy(collider.gameObject);
        }
        if (collider.tag == "Resource2")
        {
            Debug.Log("MORE SHIELDS");
            g.GetComponent<Global>().resource_shields = true;

            AudioSource.PlayClipAtPoint(g.GetComponent<Global>().gotShieldBlip, new Vector3(0, 0, 0));

            // Destroy the bullet
            Destroy(gameObject);
            // Destroy the enemy/base piece/red ufo
            Destroy(collider.gameObject);
        }
    }

}
