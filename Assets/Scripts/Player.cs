using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public float fistingCooldown = 0.5f;
    public float speed = 10;
    public float jumpForce = 4;
    public float fistForce = 3;
    public Vector3 fistForceDirectionModifier = new Vector3(0, 0.1f, 0);

    public float Lives = 3;
    public float Health = 0;

    public Rigidbody2D Rigidbody { get; private set; }
    public PlayerAnimator PlayerAnimator { get; private set; }
    public PlayerSounds PlayerSounds { get; private set; }
    public GameManager GameManager { get; private set; }

    [SerializeField]
    private Transform fist;
    [SerializeField]
    private float fistRange;

    private bool receiveInput = false;
    private bool readyToFist = true;

    private Movement movement = Movement.Idle;

    private List<Collider2D> colliders = new List<Collider2D>();

    private Vector2 fistDirection = Vector2.zero;
    private Vector2 velocityFromMovement = Vector2.zero;

    public Player Init(PlayerInputConfiguration input, PlayerSounds sounds)
    {
        PlayerInputConfiguration = input;
        receiveInput = true;
        Rigidbody = GetComponent<Rigidbody2D>();
        PlayerSounds = sounds;
        PlayerAnimator = GetComponent<PlayerAnimator>();

        PlayerAnimator.FistEnd += OnFistEnd;
        PlayerAnimator.JumpEnd += OnJumpEnd;

        var go = GameObject.FindGameObjectWithTag("GameController");
        if (go)
            GameManager = go.GetComponent<GameManager>();
        else
            Debug.LogError("GameManager does not exists or object with tag 'GameController' was not found. " +
                "Player will not use any functionality related to game manager");

        return this;
    }

    public void ApplyKnockback(Vector3 pos)
    {
        var direction = (transform.position.x > pos.x) ? Vector3.right : Vector3.left;
        Rigidbody.AddForce((direction + fistForceDirectionModifier) * fistForce * (1 + (Health / 100)), ForceMode2D.Impulse);
    }

    private void Update()
    {
        // Apply movement velocity manually
        transform.localPosition += new Vector3(velocityFromMovement.x, 0, 0) * Time.deltaTime; // This is not smooth when going into walls
        //Rigidbody.position += new Vector2(velocityFromMovement.x, 0) * Time.deltaTime; // This one is not so smooth with camera movement
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
            velocityFromMovement = Vector2.zero;
            movement = Movement.Idle;
        }
        else
        {
            int direction = (value > 0) ? 1 : -1;
            movement = (value > 0) ? Movement.Right : Movement.Left;
            velocityFromMovement = Vector2.right * speed * direction;
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
        var hit = Physics2D.CircleCast(fist.transform.position, 0.5f, new Vector2(transform.localScale.x, 0), 0.5f);
        if (hit && hit.collider && hit.collider.GetComponent<Player>())
        {
            var player = hit.collider.GetComponent<Player>();
            player.Health += 10 + player.Health / 10;
            player.ApplyKnockback(transform.position);
        }

        readyToFist = true;
        PlayerAnimator.IsFisting = false;
    }

    private void OnJumpEnd()
    {
        Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

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
        if (!readyToFist) return;
        if (Input.GetButtonDown(PlayerInputConfiguration.Fist))
        {
            readyToFist = false;
            PlayerAnimator.IsFisting = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground" && !colliders.Contains(collider))
        {
            colliders.Add(collider);
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            colliders.Remove(collider);
        }
    }
}
