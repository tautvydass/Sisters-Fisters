using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
	private bool punching = false;

	public void Punch(Vector2 direction)
	{
		if(punching) return;

		punching = true;


	}
}
	
