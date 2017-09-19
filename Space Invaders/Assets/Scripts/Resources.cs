using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour {

    public GameObject resource1;        // Lives
    public GameObject resource2;        // Shields
    public GameObject resource3;        // Bullets

    public GameObject g;

    public bool spawnedResources;

    public GameObject[] resources;

    // Use this for initialization
    void Start () {
        g = GameObject.Find("GlobalObject");

        spawnedResources = false;

        resources = new GameObject[5];
    }

    // Update is called once per frame
    void Update () {
		if(g.GetComponent<Global>().spawnResources)
        {
            if(!spawnedResources)
            {
                for (int i = 0; i < 5; i++)
                {
                    int num = Random.Range(0, 3);

                    if (num == 0)
                    {
                        resources[i] = Instantiate(resource1, new Vector3(Random.Range(-25, 25), Random.Range(-6, 20), 10.06f), Quaternion.identity).gameObject;
                    }
                    else if (num == 1)
                    {
                        resources[i] = Instantiate(resource2, new Vector3(Random.Range(-25, 25), Random.Range(-6, 20), 10.06f), Quaternion.identity).gameObject;
                    }
                    else if(num == 2)
                    {
                        resources[i] = Instantiate(resource3, new Vector3(Random.Range(-25, 25), Random.Range(-6, 20), 10.06f), Quaternion.identity).gameObject;
                    }
                }

                spawnedResources = true;

                StartCoroutine("DestroyResources");
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
        float x;
        float side = Random.Range(1, 2);
        if(side == 1)
        {
            x = Random.Range(-30, -21);
        }
        else
        {
            x = Random.Range(22, 30);
        }

        float y = Random.Range(-6, 20);
        return new Vector3(x, y, 10.06f);
    }

    IEnumerator DestroyResources()
    {
        yield return new WaitForSeconds(5);

        for (int i = 0; i < 5; i++)
        {
            Destroy(resources[i].gameObject);
        }

        yield break;
    }
}
