using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator m_Animator;

    private const string AnimatorProp_IsMovingRight = "IsMovingRight";
    private const string AnimatorProp_IsRunning = "IsRunning";
    private const string AnimatorProp_IsInAir = "IsInAir";

    void Start ()
    {
        m_Animator = GetComponent<Animator>();
	}
	
    public void LookLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        //m_Animator.SetBool(AnimatorProp_IsMovingRight, false);
    }

    public void LookRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
        // m_Animator.SetBool(AnimatorProp_IsMovingRight, true);
    }

    public bool IsRunning
    {
        set
        {
            m_Animator.SetBool(AnimatorProp_IsRunning, value);
        }
    }

    public bool IsInAir
    {
        set
        {
            m_Animator.SetBool(AnimatorProp_IsInAir, value);
        }
    }
}
