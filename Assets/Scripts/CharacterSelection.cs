using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
	[SerializeField]
	private List<Image> players;
	[SerializeField]
	private Color selected;
	[SerializeField]
	private Color backgorund;
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

	private void Initialize(int playerIndex, PlayerInputConfiguration input)
	{
		text.sprite = playerTexts[selectedPlayer];
		selectedPlayer = Random.Range(0, 4);
		Select();
		SelectionSound();
		active = true;
		this.input = input;
	}

	private void Select()
	{
		for(int i = 0; i < players.Count; i++)
			if(i != selectedPlayer)
				players[i].color = backgorund;
			else
				players[i].color = selected;
	}

	private void UpdateSelection(int direction)
	{
		selectedPlayer += direction;
		selectedPlayer %= players.Count;
		Select();
		SelectionSound();
	}

	private void Update()
	{
		if(!active) return;


	}

	private void SelectionSound()
	{
		audioSource.PlayOneShot(selectionClips[selectedPlayer]);
	}
}
