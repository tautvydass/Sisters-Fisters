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

    public float criticalChance = 0.1f;
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
    [SerializeField]
    private HitParticles hitParticles;
    [SerializeField]
    private SpriteRenderer playerNumRenderer;
    [SerializeField]
    private AudioSource audioSource;

    private bool receiveInput = false;
    private bool readyToFist = true;
    private bool canDoubleJump = true;

    private Movement movement = Movement.Idle;

    private List<Collider2D> colliders = new List<Collider2D>();

    private Vector2 jumpDirection = Vector2.zero;
    private Vector2 velocityFromMovement = Vector2.zero;

    public PlayerData data;
    [SerializeField]
    private AudioClip criticalHit;

    private PlayerHUD playerHUD;

    
    public Player Init(PlayerData data, Vector3 spawnPoint, Sprite playerNumSprite)
    {
        this.data = data;
        PlayerInputConfiguration = data.input;
        Rigidbody = GetComponent<Rigidbody2D>();
        PlayerSounds = data.sounds;
        PlayerAnimator = GetComponent<PlayerAnimator>();

        PlayerAnimator.FistEnd += OnFistEnd;
        PlayerAnimator.JumpEnd += OnJumpEnd;

        PlayerAnimator.SetCharacter(data.characterIndex);

        playerNumRenderer.sprite = playerNumSprite;
        transform.position = spawnPoint;

        hitParticles.Init(data.characterIndex);

        audioSource = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();

        playerHUD = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().SetupPlayer(data.playerIndex);

        return this;
    }

    public void Enable() =>
        receiveInput = true;

    public void ApplyKnockback(Vector3 pos, bool critical)
    {
        var direction = (transform.position.x > pos.x) ? Vector3.right : Vector3.left;
        Rigidbody.AddForce((direction + fistForceDirectionModifier) * fistForce * (1 + (Health / 100)) * (critical ? 2 : 1), ForceMode2D.Impulse);
        audioSource.PlayOneShot(PlayerSounds.getHit, 0.8f);
    }

    public void RemoveHeart() =>
        playerHUD.RemoveHeart();

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
        CheckJumpDirection();
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
        bool critical;
        var hit = Physics2D.CircleCast(fist.transform.position, 0.5f, new Vector2(transform.localScale.x, 0), 0.5f);
        if (hit && hit.collider && hit.collider.GetComponent<Player>())
        {
            critical = UnityEngine.Random.Range(0f, 1f) < criticalChance;
            var player = hit.collider.GetComponent<Player>();
            hitParticles.Emit(critical ? 100 : 40);
            player.Health += 10 + player.Health / 10;
            player.ApplyKnockback(transform.position, critical);
            if(critical)
                audioSource.PlayOneShot(criticalHit, 1.0f);
            else
                audioSource.PlayOneShot(PlayerSounds.punches[UnityEngine.Random.Range(0, PlayerSounds.punches.Count)], 0.8f);
        }
        readyToFist = true;
        PlayerAnimator.IsFisting = false;
    }

    private void OnJumpEnd()
    {
        Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        audioSource.PlayOneShot(PlayerSounds.jump, 0.8f);
    }

    private void CheckJumpDirection() =>
        jumpDirection = (new Vector2(Input.GetAxis(PlayerInputConfiguration.LookHorizontal), -Input.GetAxis(PlayerInputConfiguration.LookVertical))).normalized;

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
            else if(canDoubleJump)
            {
                DoubleJump();
                canDoubleJump = false;
            }
    }

    private void DoubleJump()
    {
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0);
        Rigidbody.AddForce((jumpDirection + Vector2.up) / 2 * jumpForce * 0.75f, ForceMode2D.Impulse);
        audioSource.PlayOneShot(PlayerSounds.jump, 0.8f);
    }

    private void CheckFist()
    {
        if (!readyToFist) return;
        if (Input.GetButtonDown(PlayerInputConfiguration.Fist))
        {
            readyToFist = false;
            PlayerAnimator.IsFisting = true;
            audioSource.PlayOneShot(PlayerSounds.hit, 0.8f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground" && !colliders.Contains(collider))
        {
            if(colliders.Count == 0)
                canDoubleJump = true;
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
