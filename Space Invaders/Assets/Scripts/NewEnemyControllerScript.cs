using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class handles individual enemy objects.
 */
public class NewEnemyControllerScript : MonoBehaviour {

    public int pointValue;

	// Use this for initialization
	void Start () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
