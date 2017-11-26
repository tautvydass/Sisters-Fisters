using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class PlayerHUD : MonoBehaviour
{
	[SerializeField]
	private List<Image> hearts;

	[SerializeField]
	private Color disabledColor;


    [SerializeField]
    private Text healthText;

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

    public void DisplayHealth(float health)
    {
        healthText.text = ((int)health) + " %";
    }
}
