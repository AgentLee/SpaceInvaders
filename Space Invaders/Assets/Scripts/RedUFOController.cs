using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedUFOController : MonoBehaviour
{
	private Transform ufo;
	public float speed;

	public Vector3 startPos;

	public GameObject g;

	// Use this for initialization
	void Start () 
	{
		g = GameObject.Find ("GlobalObject");

		InvokeRepeating ("MoveEnemy", 0.1f, 0.3f);

		ufo = GetComponent<Transform> ();

		if (Mathf.Ceil (Random.Range (0.0f, 10.0f)) % 2 == 0) {
			startPos = new Vector3 (-30.0f, 10.5f, 0.0f);
		} 
		else {
			startPos = new Vector3 (30.0f, 10.5f, 0.0f);
		}

		ufo.position = startPos;
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
