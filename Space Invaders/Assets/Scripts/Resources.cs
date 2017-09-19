using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour {

    public GameObject resource1;        // Lives
    public GameObject resource2;        // Shields
    public GameObject resource3;        // Bullets

    public GameObject g;

    public bool spawnedResources;

    // Use this for initialization
    void Start () {
        g = GameObject.Find("GlobalObject");

        spawnedResources = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(g.GetComponent<Global>().spawnResources)
        {
            if(!spawnedResources)
            {
                SpawnResources();
            }

            // Randomly spawn lives, shields, or bullets

            // Lives
            // If the user already has 5 lives then they can get a random
            // amount of points by hitting the resource.

            // Shields
            // The players' bases get more protection.
            // Their health can increase preventing them from being destroyed quicker.

            // Bullets
            // Burst fire or missile
        }
	}

    Vector3 RandomPosition()
    {
        float x = Random.Range(-25, 25);
        float y = Random.Range(-6, 28);
        return new Vector3(x, y, 10.06f);
    }

    public GameObject[] resources;
    IEnumerator SpawnResources()
    {
        resources = new GameObject[5];
        for(int i = 0; i < 5; i++)
        {
            int num = Random.Range(0, 2);

            switch(num)
            {
                case 0:
                    resources[i] = Instantiate(resource1, RandomPosition(), Quaternion.identity);
                    break;
                case 1:
                    resources[i] = Instantiate(resource2, RandomPosition(), Quaternion.identity);
                    break;
                case 2:
                    resources[i] = Instantiate(resource3, RandomPosition(), Quaternion.identity);
                    break;
                default:
                    break;
            }
        }

		yield return new WaitForSeconds (10);

        for(int i = 0; i < 5; i++)
        {
            Destroy(resources[i].gameObject);
        }
    }
}
