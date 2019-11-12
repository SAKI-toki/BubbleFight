using UnityEngine;

/// <summary>
/// ダメージ計算をする
/// </summary>
static class DamageCalculator
{
    const float HitDamageWeight = 0.65f;
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

    void Start()
    {
        currentHitPoint = maxHitPoint;
    }

    void Update()
    {
        if (IsBreak())
        {
            Broken();
        }
    }

    /// <summary>
    /// 耐久値の最大値
    /// </summary>
    public float GetMaxHitPoint()
    {
        return maxHitPoint;
    }

    /// <summary>
    /// 現在の耐久値
    /// </summary>
    public float GetCurrentHitPoint()
    {
        return currentHitPoint;
    }

    /// <summary>
    /// 壊れるかどうか(耐久値が0以下)
    /// </summary>
    public bool IsBreak()
    {
        return currentHitPoint <= 0;
    }

    /// <summary>
    /// 破壊プログラム
    /// </summary>
    public void Broken()
    {
        Destroy(this.gameObject);
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
}