using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedUFOController : MonoBehaviour
{
	private Transform ufo;
	public float speed;
	public int[] pointValues;
	public Vector3 startPos;
	public bool hit;

	public GameObject g;

	// Use this for initialization
	void Start () 
	{
		g = GameObject.Find ("GlobalObject");

		hit = false;

		InvokeRepeating ("MoveEnemy", 0.1f, 0.3f);

		ufo = GetComponent<Transform> ();

		if (Mathf.Ceil (Random.Range (0.0f, 10.0f)) % 2 == 0) {
			startPos = new Vector3 (-30.0f, 14f, 9.95f);
		} 
		else {
			startPos = new Vector3 (30.0f, 14f, 9.95f);
		}

		pointValues = new int[3];
		pointValues [0] = 50;
		pointValues [1] = 100;
		pointValues [2] = 150;

		ufo.position = startPos;
	}

	void Update()
	{
		if (hit) {
			g.GetComponent<Global> ().hitRedUFO = true;
		}
	}

	void MoveEnemy()
	{
		MeshRenderer render = ufo.gameObject.GetComponentInChildren<MeshRenderer> ();

		// Move left to right
		if (startPos.x < 0.0f) {

			if (render.isVisible) {
				ufo.position += Vector3.right * speed;
				g.GetComponent<Global> ().spawnedRedUFO = true;
			} else {
				Destroy (ufo.gameObject);
				g.GetComponent<Global> ().spawnedRedUFO = false;
			}
		}
		// Move right to left
		else {
			if (render.isVisible) {
				ufo.position += Vector3.left * speed;
				g.GetComponent<Global> ().spawnedRedUFO = true;
			} else {
				Destroy (ufo.gameObject);
				g.GetComponent<Global> ().spawnedRedUFO = false;
			}
		}
	}
}
