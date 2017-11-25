using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
	private AsyncOperation load;

	private void Awake()
	{
		DontDestroyOnLoad(this);
		SceneManager.LoadScene("Splash");
	}

	private void Start() =>
		Preload();

	public void LoadSelection(float transitionTime)
	{
		// transit here
		StartCoroutine(FromSplashToSelection(transitionTime));
	}

	private void Preload()
	{
		load = SceneManager.LoadSceneAsync("Character Selection", LoadSceneMode.Additive);
		load.allowSceneActivation = false;
	}

	private IEnumerator FromSplashToSelection(float transitionTime)
	{
		yield return new WaitForSeconds(transitionTime);
		load.allowSceneActivation = true;
	}
}
