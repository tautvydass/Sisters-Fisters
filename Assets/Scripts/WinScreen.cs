using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class WinScreen : MonoBehaviour
{
	[SerializeField]
	private Image character;
	[SerializeField]
	private Image player;
	[SerializeField]
	private Image winner;

	[SerializeField]
	private List<Sprite> characterSprites;
	[SerializeField]
	private List<Sprite> playerSprites;
	[SerializeField]
	private Text info;

	public void AnnounceWinner(int playerIndex, int characterIndex, Action victoryChantCallback)
	{
		character.sprite = characterSprites[characterIndex];
		player.sprite = playerSprites[playerIndex];
		var count = transform.childCount;
		for(int i = 0; i < count; i++)
			transform.GetChild(i).gameObject.SetActive(true);
		StartCoroutine(WaitForInput(victoryChantCallback));
	}

	private IEnumerator WaitForInput(Action victoryChantCallback)
	{
		yield return new WaitForSeconds(0.5f);
		victoryChantCallback();
		yield return new WaitForSeconds(2.5f);
		info.enabled = true;
		while(true)
		{
			if(Input.anyKeyDown)
				SceneManager.LoadScene("Character Selection");
			yield return 0;
		}
	}
}
