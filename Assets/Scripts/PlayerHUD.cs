using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerHUD : MonoBehaviour
{
	[SerializeField]
	private List<Image> hearts;

	[SerializeField]
	private Color disabledColor;

	public PlayerHUD Init(int playerIndex)
	{
		foreach(var heart in hearts)
			heart.color = Properties.PLAYER_COLORS[playerIndex];
		return this;
	}

	public void RemoveHeart()
	{
		hearts.Last().color = disabledColor;
		hearts.Remove(hearts.Last());
	}
}
