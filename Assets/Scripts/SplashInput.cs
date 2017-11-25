using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashInput : MonoBehaviour
{
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		else if(Input.anyKeyDown)
			Load();
	}

	private void Load() =>
		SceneManager.LoadScene("Character Selection");
}
