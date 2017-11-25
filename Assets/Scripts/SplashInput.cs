using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashInput : MonoBehaviour
{
	[SerializeField]
	private Animator fadeAnimator;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		else if(Input.anyKeyDown)
			Load();
	}

	private void Load()
	{
		fadeAnimator.enabled = true;
		GameObject.FindGameObjectWithTag("TManager").GetComponent<TransitionManager>().LoadSelection(1.0f);
	}
}
