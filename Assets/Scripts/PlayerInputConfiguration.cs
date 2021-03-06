﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sisters Fisters", fileName = "Player Input Configuration")]
public class PlayerInputConfiguration : ScriptableObject
{
	public string Horizontal;
	public string LookHorizontal;
	public string LookVertical;
	public string Select;
	public string Jump;
	public string Fist;
}
