﻿using UnityEngine;

/// <summary>
/// ダメージ計算をする
/// </summary>
static class DamageCalculator
{
    const float HitDamageWeight = 0.3f;
    /// <summary>
    /// ダメージを計算し返す
    /// </summary>
    static public float Damage(float collisionPower, float damagePercent)
    {
        return Mathf.Pow(collisionPower * damagePercent, HitDamageWeight);
    }
}

/// <summary>
/// 壊れるオブジェクト
/// </summary>
public class BrokenObject : MonoBehaviour
{
    [SerializeField, Tooltip("耐久値")]
    float maxHitPoint = 100.0f;
    //現在の耐久値
    float currentHitPoint;
    protected bool alreadyBroken = false;

    [SerializeField]
    bool debugDestroy = false;

    void Start()
    {
        SetMaxHp();
    }

    void Update()
    {
        if (debugDestroy)
        {
            debugDestroy = false;
            currentHitPoint = 0;
        }
        if (!alreadyBroken && IsBreak())
        {
            alreadyBroken = true;
            Broken();
        }
    }

    /// <summary>
    /// 壊れるかどうか(耐久値が0以下)
    /// </summary>
    bool IsBreak()
    {
        return currentHitPoint <= 0;
    }

    /// <summary>
    /// 最大HPをセット
    /// </summary>
    protected void SetMaxHp()
    {
        currentHitPoint = maxHitPoint;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            currentHitPoint -= DamageCalculator.Damage(other.relativeVelocity.sqrMagnitude, 1.0f);
        }
        //マップ外に出た時の処理
        if (other.gameObject.tag == "BreakArea")
        {
            currentHitPoint = 0.0f;
        }
    }

    /// <summary>
    /// 破壊
    /// </summary>
    protected virtual void Broken()
    {
        Destroy(gameObject);
    }
}