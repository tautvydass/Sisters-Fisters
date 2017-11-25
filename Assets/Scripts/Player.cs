using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum Movement
    {
        Left,
        Right,
        Idle
    }

    public PlayerInputConfiguration PlayerInputConfiguration { get; private set; }

    public float speed = 10;
    public float jumpForce = 4;
    public float FistForce { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public PlayerAnimator PlayerAnimator { get; private set; }

    private bool receiveInput = false;

    private Movement movement = Movement.Idle;

    private List<Collider2D> colliders = new List<Collider2D>();

    private Vector2 fistDirection = Vector2.zero;

    public Player Init(PlayerInputConfiguration input)
    {
        PlayerInputConfiguration = input;
        receiveInput = true;
        Rigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<PlayerAnimator>();

        PlayerAnimator.FistEnd += OnFistEnd;
        PlayerAnimator.JumpEnd += OnJumpEnd;

        return this;
    }

    private void LateUpdate()
    {
        if (!receiveInput) return;

        CheckHorizontal();
        CheckFistDirection();
        CheckJump();
        CheckFist();
        ApplyAnimations();
    }

    private void CheckHorizontal()
    {
        var value = Input.GetAxis(PlayerInputConfiguration.Horizontal);
        if (Mathf.Abs(value) != 1)
        {
            if (movement == Movement.Idle) return;
            if (movement == Movement.Left)
                Move(1);
            else if (movement == Movement.Right)
                Move(-1);
            movement = Movement.Idle;
        }
        else if (value == 1)
        {
            if (movement == Movement.Right) return;
            if (movement == Movement.Left)
                Move(2);
            else if (movement == Movement.Idle)
                Move(1);
            movement = Movement.Right;
        }
        else if (value == -1)
        {
            if (movement == Movement.Left) return;
            if (movement == Movement.Right)
                Move(-2);
            else if (movement == Movement.Idle)
                Move(-1);
            movement = Movement.Left;
        }
    }

    private void ApplyAnimations()
    {
        PlayerAnimator.IsRunning = movement != Movement.Idle;
        PlayerAnimator.IsInAir = !IsGrounded();

        // Looking direction
        if (movement == Movement.Right)
            PlayerAnimator.LookRight();
        else if (movement == Movement.Left)
            PlayerAnimator.LookLeft();
    }

    private void OnFistEnd()
    {
        // Add damage here
    }

    private void OnJumpEnd()
    {
        Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Move(float direction) =>
        Rigidbody.velocity += Vector2.right * direction * speed;

    private void CheckFistDirection() =>
        fistDirection = (new Vector2(Input.GetAxis(PlayerInputConfiguration.LookHorizontal), -Input.GetAxis(PlayerInputConfiguration.LookVertical))).normalized;

    private bool IsGrounded() =>
        colliders.Count != 0;

    private void CheckJump()
    {
        if (Input.GetButtonDown(PlayerInputConfiguration.Jump))
            if (IsGrounded())
            {
                PlayerAnimator.Jump();
                // Set some variables that it is jumping so movement becomes disabled
            }
    }
        
    private void CheckFist()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "Ground") return;
        if (colliders.Contains(collider)) return;

        colliders.Add(collider);
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        colliders.Remove(collider);
    }
}
