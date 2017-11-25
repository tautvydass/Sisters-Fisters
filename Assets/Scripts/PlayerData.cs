using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
	public PlayerInputConfiguration input;
	public int characterIndex;
	public int playerIndex;
	public PlayerSounds sounds;

	public PlayerData(PlayerInputConfiguration input, int characterIndex, int playerIndex, PlayerSounds sounds)
	{
		this.input = input;
		this.characterIndex = characterIndex;
		this.playerIndex = playerIndex;
		this.sounds = sounds;
	}
}
