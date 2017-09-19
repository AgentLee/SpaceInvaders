using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class handles individual enemy objects.
 */
public class NewEnemyControllerScript : MonoBehaviour {

    public int pointValue;
    public GameObject g;
	// Use this for initialization
	void Start () {
        g = GameObject.Find("GlobalObject");
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            g.GetComponent<Global>().numEnemies--;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
