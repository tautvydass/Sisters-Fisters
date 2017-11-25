using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public void AnnounceWinner(int playerIndex, int characterIndex)
	{
		character.sprite = characterSprites[characterIndex];
		player.sprite = playerSprites[playerIndex];
	}
}
