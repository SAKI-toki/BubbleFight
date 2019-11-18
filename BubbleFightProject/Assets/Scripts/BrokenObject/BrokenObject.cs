using UnityEngine;

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
    [SerializeField, Tooltip("リスポーンするかどうか")]
    bool isRespawn = false;
    [SerializeField, Tooltip("リスポーンの間隔")]
    float respawnIntervalTime = 0.0f;
    [SerializeField, Tooltip("リスポーンする場合、レンダラーをオフにする")]
    Renderer[] renderers = null;
    float respawnTimeCount = 0.0f;

    //現在の耐久値
    float currentHitPoint;

    bool alreadyBroken = false;

    void Start()
    {
        currentHitPoint = maxHitPoint;
    }

    void Update()
    {
        if (!alreadyBroken && IsBreak())
        {
            alreadyBroken = true;
            Broken();
        }
        if (alreadyBroken && isRespawn)
        {
            respawnTimeCount -= Time.deltaTime;
            if (respawnTimeCount < 0.0f)
            {
                alreadyBroken = false;
                foreach (var renderer in renderers)
                {
                    renderer.enabled = true;
                }
                foreach (var collider in GetComponents<Collider>())
                {
                    collider.enabled = true;
                }
                currentHitPoint = maxHitPoint;
            }
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
        if (isRespawn)
        {
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;
            }
            foreach (var collider in GetComponents<Collider>())
            {
                collider.enabled = false;
            }
            respawnTimeCount = respawnIntervalTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
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