using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;

    public AnimState CurrentAnimState;

    public event Action FistEnd;
    public event Action JumpEnd;

    private bool m_IsJumping;
    private float m_StartingXScale;
    [SerializeField]
    private Transform playerNumber;

    private bool m_IsFisting;
    public bool IsFisting
    {
        set
        {
            m_IsFisting = value;
            UpdateAnimator();
        }
    }

    private bool m_IsRunning;
    public bool IsRunning
    {
        set
        {
            m_IsRunning = value;
            UpdateAnimator();
        }
    }

    private bool m_IsInAir;
    public bool IsInAir
    {
        set
        {
            m_IsInAir = value;
            UpdateAnimator();
        }
    }

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_StartingXScale = transform.localScale.x;
    }

    public void SetCharacter(int characterID)
    {
        m_Animator.SetFloat("PlayerID", characterID);
        UpdateAnimator();
    }

    public void LookLeft()
    {
        //m_SpriteRenderer.flipX = true;
        playerNumber.localScale = new Vector3(-1, 1, 1);
        transform.localScale = new Vector3(-m_StartingXScale, transform.localScale.y, transform.localScale.z);
    }

    public void LookRight()
    {
        //m_SpriteRenderer.flipX = false;
        playerNumber.localScale = Vector3.one;
        transform.localScale = new Vector3(m_StartingXScale, transform.localScale.y, transform.localScale.z);

    }

    public void Jump()
    {
        m_IsJumping = true;
        UpdateAnimator();
    }

    /// <summary>
    /// Called magically from animation event
    /// </summary>
    public void JumpEndCallback()
    {
        JumpEnd?.Invoke();
        m_IsJumping = false;
        UpdateAnimator();
    }

    /// <summary>
    /// Called magically from animation event
    /// </summary>
    private void FistEndCallback()
    {
        FistEnd?.Invoke();
        IsFisting = false;
        UpdateAnimator();
    }

    public void UpdateAnimator()
    {
        CurrentAnimState = AnimState.Idle;

        // Execution order is important here because one animations have priority over each other
        if (m_IsJumping)
            CurrentAnimState = AnimState.Jumping;
        else if (m_IsFisting)
            CurrentAnimState = AnimState.Fisting;
        else if (m_IsInAir)
            CurrentAnimState = AnimState.Air;
        else if (m_IsRunning)
            CurrentAnimState = AnimState.Running;

        m_Animator.SetFloat("State", (int)CurrentAnimState);
        m_Animator.SetInteger("IntState", (int)CurrentAnimState);
    }

    public enum AnimState
    {
        Idle = 0,
        Running = 1,
        Fisting = 2,
        Jumping = 3,
        Air = 4
    }
}
