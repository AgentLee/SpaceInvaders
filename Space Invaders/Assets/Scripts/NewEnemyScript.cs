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
    public bool hordeStart;

    public GameObject g;

    public bool freeze;

    public GameObject horder;

    public bool spawnedHorde;

    // Use this for initialization
    void Start () {
		InvokeRepeating ("MoveEnemy", 0.1f, 0.3f);

        g = GameObject.Find("GlobalObject");

        enemies = GetComponent<Transform>();

        freeze = false;
        hordeStart = false;

        spawnedHorde = false;
    }

    public int enemiesBreached;

    // Update is called once per frame
    void Update() {
        if (hordeStart)
        {
            if (!spawnedHorde && partOfHorde)
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(-15, 15), Random.Range(22, 1000), 10.0625f);
                    Instantiate(horder, pos, Quaternion.identity).transform.parent = enemies;
                }

                spawnedHorde = true;
            }
            //AudioSource.PlayClipAtPoint(g.GetComponent<Global>().lightspeed, new Vector3(0, 0, 0));

            StartCoroutine("HordeAttack");
        }
        else if(!hordeStart)
        {
            StopCoroutine("HordeAttack");
        }
	}

    void FixedUpdate()
    {
        hordeStart = g.GetComponent<Global>().hordeStart;

        if(!partOfHorde)
        {
            // Hide during the horde
            if(hordeStart)
            {
                foreach (Transform enemy in enemies)
                {
                    MeshRenderer render = enemy.gameObject.GetComponentInChildren<MeshRenderer>();
                    render.enabled = false;
                }
            }
            // Show when the horde finishes
            else
            {
                foreach (Transform enemy in enemies)
                {
                    MeshRenderer render = enemy.gameObject.GetComponentInChildren<MeshRenderer>();
                    render.enabled = true;
                }
            }
        }
    }

    public int enemiesDestroyed;
    void MoveEnemy()
    {
        // True when the player died and is in the middle of respawn.
        freeze = g.GetComponent<Global> ().freeze;

        if(!freeze) {
            // If not part of the horde (game level enemies), then they 
            // should stop moving and shooting. 
            if(!partOfHorde) {
                if(!hordeStart) {
                    enemies.position += Vector3.right * speed;

                    // Moves them down when they hit the boundaries.
                    if (enemies.position.x < -15 || enemies.position.x > 15)
                    {
                        // Have them move left to right, right to left
                        speed = -speed;
                        // Move them down
                        enemies.position += Vector3.down * 0.5f;

                        return;
                    }
                }
            }
            // Horde should keep moving even if there is a freeze.
            // This punishes the player for dying and shouldn't get any points 
            // during their respawn time. :P
            else
            {
                //StartCoroutine("HordeAttack");
            }

            if (!hordeStart && shouldBeShooting) {
                // This allows each individual enemy to shoot bullets.
                foreach (Transform enemy in enemies)
                {
                    if (Random.value > fireRate && shouldBeShooting)
                    {
                        AudioSource.PlayClipAtPoint(blasterSound, gameObject.transform.position);

                        // Bullet spawn position.
                        Vector3 pos = enemy.transform.position;
                        // Offset just a little from the enemy.
                        pos.y -= 1.0f;
                        Instantiate(shot, pos, this.transform.rotation);
                    }
                }
            }
        }
    }

    IEnumerator HordeAttack()
    {
        g.GetComponent<Global>().hyperspace.Play();
        if(partOfHorde)
        {
            foreach (Transform enemy in enemies)
            {
                enemy.position += Vector3.down * (speed);

                if (enemy.position.y < -20)
                {
                    g.GetComponent<Global>().score--;
                    g.GetComponent<Global>().enemiesBreached++;
                    Destroy(enemy.gameObject);
                }
            }
        }

		yield return new WaitForSeconds (20);

        // See if there's a way to have them keep moving down.
        // Otherwise keep it like this for now.
        int destroyed = 0;
        if (partOfHorde)
        {
            foreach (Transform enemy in enemies)
            {
                Destroy(enemy.gameObject);
                destroyed++;
            }
        }

        hordeStart = false;
        spawnedHorde = false;
        g.GetComponent<Global>().hordeStart = false;
        g.GetComponent<Global>().hordeTimer = 5;
        g.GetComponent<Global>().playedBarrierDestroyedClip = false;

        g.GetComponent<Global>().endedHordeAttack = true;
        g.GetComponent<Global>().hyperspace.Stop();

        yield break;
    }
}