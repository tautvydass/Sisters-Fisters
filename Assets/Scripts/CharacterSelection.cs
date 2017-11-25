using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
	[SerializeField]
	private List<Image> players;
	[SerializeField]
	private Color selectedColor;
	[SerializeField]
	private Color backgorundColor;
	[SerializeField]
	private Image text;
	[SerializeField]
	private List<Sprite> playerTexts;
	[SerializeField]
	private List<AudioClip> selectionClips;
	[SerializeField]
	private AudioSource audioSource;

	private PlayerInputConfiguration input;
	private int selectedPlayer;
	private bool active = false;
	private bool selected = false;

	public void SetPosition(Vector2 position)
	{
		GetComponent<RectTransform>().localPosition = position;
	}

	private void Initialize(int playerIndex, PlayerInputConfiguration input)
	{
		text.sprite = playerTexts[selectedPlayer];
		selectedPlayer = Random.Range(0, 4);
		Select();
		//SelectionSound();
		active = true;
		this.input = input;
	}

	private void Select()
	{
		for(int i = 0; i < players.Count; i++)
			if(i != selectedPlayer)
				players[i].color = backgorundColor;
			else
				players[i].color = selectedColor;
	}

	private void UpdateSelection(int direction)
	{
		selectedPlayer += direction;
		selectedPlayer %= players.Count;
		Select();
		//SelectionSound();
	}

	private void Update()
	{
		if(!active) return;

		var selection = Input.GetAxis(input.Select);
		
		if(Mathf.Abs(selection) != 1)
		{
			if(selected) selected = false;
		}
		else if(selection == 1)
		{
			if(selected) return;

			UpdateSelection(1);
			selected = true;
		}
		else if(selection == -1)
		{
			if(selected) return;

			UpdateSelection(-1);
			selected = true;
		}
	}

	private void SelectionSound()
	{
		audioSource.PlayOneShot(selectionClips[selectedPlayer]);
	}
}
