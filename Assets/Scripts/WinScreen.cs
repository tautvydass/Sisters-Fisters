using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	public void AnnounceWinner(int playerIndex, int characterIndex)
	{
		character.sprite = characterSprites[characterIndex];
		player.sprite = playerSprites[playerIndex];
		var count = transform.childCount;
		for(int i = 0; i < count; i++)
			transform.GetChild(i).gameObject.SetActive(true);
		StartCoroutine(WaitForInput());
	}

	private IEnumerator WaitForInput()
	{
		yield return new WaitForSeconds(1);
		info.enabled = true;
		while(true)
		{
			if(Input.anyKeyDown)
				SceneManager.LoadScene("Character Selection");
			yield return 0;
		}
	}
}
