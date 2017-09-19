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
            //if(!spawnedResources)
            //{
                StartCoroutine("DestroyResources");
            //}
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
        for (int i = 0; i < 5; i++)
        {
            int num = Random.Range(0, 2);

            if (num == 0)
            {
                resources[i] = Instantiate(resource1, new Vector3(Random.Range(-30, 30), Random.Range(-6, 20), 10.06f), Quaternion.identity).gameObject;
            }
            else if (num == 1)
            {
                resources[i] = Instantiate(resource2, new Vector3(Random.Range(-30, 30), Random.Range(-6, 20), 10.06f), Quaternion.identity).gameObject;
            }
        }

        g.GetComponent<Global>().spawnResources = false;
        yield return new WaitForSeconds(5);

        for (int i = 0; i < 5; i++)
        {
            Destroy(resources[i].gameObject);
        }

        spawnedResources = true;

        yield break;
    }
}
