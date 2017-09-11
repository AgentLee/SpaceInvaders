using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour 
{
	public void MenuButton(string scene)
	{
		if (scene == "quit") {
			Application.Quit ();
		}

		SceneManager.LoadScene (scene);
	}
}
