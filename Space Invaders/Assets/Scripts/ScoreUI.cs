using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour 
{
	Global globalObj;
	Text scoreText;

	int score;

	// Use this for initialization
	void Start () 
	{
		GameObject g = GameObject.Find ("GlobalObject");
		globalObj = g.GetComponent<Global> ();

		scoreText = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		scoreText.text = "Score: " + globalObj.score.ToString ();
	}
}
