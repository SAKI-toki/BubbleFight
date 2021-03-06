﻿using UnityEngine;

/// <summary>
/// プレイヤーのアニメーションを制御するクラス
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField, Tooltip("アニメーター")]
    Animator animator = null;

    AnimatorStateInfo animatorStateInfo;

    void Update()
    {
        NoLoopAnimationSwitch();
    }

    /// <summary>
    /// アニメーション種類
    /// </summary>
    public enum AnimationType
    {
        Idle,
        Walk,
        Run,
        Jump,
        Eat,
        Rest
    }

    /// <summary>
    /// アニメーション切り替え
    /// </summary>
    public void AnimationSwitch(AnimationType type)
    {
        if (animator == null) return;
        switch (type)
        {
            case AnimationType.Idle:
                animator.SetInteger("State", 0);
                break;
            case AnimationType.Walk:
                animator.SetInteger("State", 1);
                break;
            case AnimationType.Run:
                animator.SetInteger("State", 2);
                break;
            case AnimationType.Jump:
                animator.SetInteger("State", 3);
                break;
            case AnimationType.Eat:
                animator.SetInteger("State", 4);
                break;
            case AnimationType.Rest:
                animator.SetInteger("State", 5);
                break;
        }
    }

    /// <summary>
    /// アニメーションスピードの設定
    /// </summary>
    public void AnimationSpeed(float speed)
    {
        animator.speed = speed;
    }

    /// <summary>
    /// ループしないアニメーションの遷移
    /// </summary>
    public void NoLoopAnimationSwitch()
    {
        if (!animator) return;
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ループしないアニメーションだったら
        if (!animatorStateInfo.loop)
        {
            // 1回ループしたら
            if (animatorStateInfo.normalizedTime > 1)
            {
                AnimationSwitch(AnimationType.Idle);
            }
        }
    }
}
