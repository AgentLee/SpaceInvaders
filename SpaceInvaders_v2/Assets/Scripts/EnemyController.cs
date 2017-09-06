using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base enemy class
// There are 3 types of enemies that are made as prefabs
// and are given 10, 20, 30 pointValues.
public class EnemyController : MonoBehaviour
{
	public int pointValue;

	// Use this for initialization
	void Start () 
	{
		
	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log (pointValue);
	}
}
