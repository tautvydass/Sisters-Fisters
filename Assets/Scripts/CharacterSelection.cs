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
	private AudioSource audioSource;
	[SerializeField]
	private Text infoText;
	[SerializeField]
	private AudioClip joinClip;
	[SerializeField]
	private List<AudioClip> selectClips;

	private PlayerInputConfiguration input;
	private int selectedPlayer;
	private bool active = false;
	private bool selected = false;
	private bool lockedIn = false;

	public void SetPosition(Vector2 position)
	{
		GetComponent<RectTransform>().localPosition = position;
	}

	public CharacterSelection Initialize(int playerIndex, PlayerInputConfiguration input)
	{
		text.sprite = playerTexts[playerIndex];
		selectedPlayer = Random.Range(0, 4);
		Select();
		active = true;
		this.input = input;
		audioSource = GameObject.FindGameObjectWithTag("CSManager").GetComponent<AudioSource>();
		audioSource.PlayOneShot(joinClip, 1f);
		return this;
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
		if(selectedPlayer == -1)
			selectedPlayer = 3;
		else if(selectedPlayer == 4)
			selectedPlayer = 0;
		Select();
		SelectionSound();
	}

	private void Update()
	{
		if(!active) return;
		if(lockedIn) return;

		var selection = Input.GetAxis(input.Select);

		if(Input.GetButtonDown(input.Jump)) LockIn();
		
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
		audioSource.PlayOneShot(selectClips[Random.Range(0, selectClips.Count)], 0.8f);
	}

	private void LockIn()
	{
		infoText.text = "- Ready -";
		infoText.GetComponent<Animator>().enabled = false;
		lockedIn = true;
		infoText.color += new Color(0, 0, 0, 1);
		DisablePlayers();
		LockInSound();
	}
	private void LockInSound()
	{
		audioSource.PlayOneShot(selectionClips[selectedPlayer], 0.8f);
	}

	private void DisablePlayers()
	{
		for(int i = 0; i < players.Count; i++)
			if(i != selectedPlayer)
				players[i].color -= new Color(0, 0, 0, 1);
			else
			{
				var transf = players[i].GetComponent<RectTransform>();
				transf.localPosition = new Vector2(-10, transf.localPosition.y);
			}
	}
}
