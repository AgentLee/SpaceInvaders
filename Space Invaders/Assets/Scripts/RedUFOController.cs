using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedUFOController : MonoBehaviour
{
	public GameObject ufo;

	public float maxTime = 5;
	public float minTime = 2;

	private float currTime;
	private float spawnTime;

	public float speed = 0.25f;

	public bool spawned;

	// Use this for initialization
	void Start () 
	{
		InvokeRepeating ("Move", 0.1f, 0.3f);
		//Instantiate (ufo, new Vector3 (15, 0, 0), Quaternion.identity);
		//SetRandomTime ();
		//currTime = minTime;

		//spawned = false;
	}
	
	void FixedUpdate () 
	{
		//currTime += Time.deltaTime;

		//if (currTime >= spawnTime) {
		//	if (!spawned) {
		//		SpawnObject ();
		//	}

		//	SetRandomTime ();
		//}
	}

	void Update()
	{
		if (spawned) {
			Move ();
		}
	}

	void SpawnObject()
	{
		currTime = 0;

		if (!spawned) {
			Instantiate (ufo, new Vector3 (15, 0, 0), Quaternion.identity);
			spawned = true;

			Debug.Log (spawned);
		} 


	}

	void SetRandomTime()
	{
		spawnTime = Random.Range (minTime, maxTime);
	}

	void Move()
	{
		//ufo.transform.position += Vector3.right * speed;

		gameObject.transform.position += Vector3.right * speed;
	}
}
