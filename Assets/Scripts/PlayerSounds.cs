using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sisters Fisters/Sounds", fileName = "New Player Sounds")]
public class PlayerSounds : ScriptableObject
{
	public AudioClip selection;
	public AudioClip jump;
	public AudioClip hit;
	public AudioClip getHit;
	public AudioClip victory;

	public List<AudioClip> punches;
}
