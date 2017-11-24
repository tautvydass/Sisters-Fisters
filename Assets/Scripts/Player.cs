using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public PlayerInputConfiguration PlayerInputConfiguration { get; private set; }

	public float speed = 10;
	public float jumpForce = 4;
	public float FistForce { get; private set; }
	public Rigidbody2D Rigidbody { get; private set; }
	
	private ConstantForce2D movementForce;

	private bool receiveInput = false;
	private bool moving = false;

	public Player Init(PlayerInputConfiguration input)
	{
		movementForce = GetComponent<ConstantForce2D>();
		PlayerInputConfiguration = input;
		receiveInput = true;
		Rigidbody = GetComponent<Rigidbody2D>();
		return this;
	}

	private void LateUpdate()
	{
		if(!receiveInput) return;

		CheckHorizontal();
		CheckJump();
		CheckFist();
	}

	private void CheckHorizontal()
	{
		var value = Input.GetAxis(PlayerInputConfiguration.Horizontal);
		if(Mathf.Abs(value) < 0.2f) value = 0;
		Move(value == 0 ? 0 : Mathf.Sign(value));
	}
	private void Move(float direction) =>
		movementForce.force = Vector2.right * direction * speed;
		
	private bool IsGrounded()
	{
		return true;
	}
	private void CheckJump()
	{
		if(Input.GetButtonDown(PlayerInputConfiguration.Jump))
			if(IsGrounded())
				Jump();
	}
	private void Jump() =>
		Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	private void CheckFist()
	{

	}
}
