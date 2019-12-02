using UnityEngine;


/// <summary>
/// ボールの情報を設定する
/// </summary>
[CreateAssetMenu]
public class BallScriptableObject : ScriptableObject
{
    [SerializeField, Tooltip("ボールの耐久値(初期値)")]
    float maxHitPoint = 100;
    public float MaxHitPoint { get { return maxHitPoint; } }

    [SerializeField, Tooltip("最大の回転力")]
    float maxAngularVelocity = 10.0f;
    public float MaxAngularVelocity { get { return maxAngularVelocity; } }

    //反発時の力の追加
    [SerializeField, Tooltip("ボール同士でぶつかったときの反発の追加率(cantInputHitPower以上の力)")]
    float strongHitBounceAddPower = 1.2f;
    public float StrongHitBounceAddPower { get { return strongHitBounceAddPower; } }

    [SerializeField, Tooltip("ボール同士でぶつかったときの反発の追加率(cantInputHitPower以下の力)")]
    float weakHitBounceAddPower = 1.2f;
    public float WeakHitBounceAddPower { get { return weakHitBounceAddPower; } }

    [SerializeField, Tooltip("最大の力")]
    float maxVelocityMagnitude = 100.0f;
    public float MaxVelocityMagnitude { get { return maxVelocityMagnitude; } }


    [SerializeField, Tooltip("入力を受け付けなくする衝突力")]
    float cantInputHitPower = 50.0f;
    public float CantInputHitPower { get { return cantInputHitPower; } }

    [SerializeField, Tooltip("入力を受け付けない最大時間")]
    float maxCantInputTime = 1.0f;
    public float MaxCantInputTime { get { return maxCantInputTime; } }

    [SerializeField, Tooltip("力1に対してどのくらい入力を受け付けなくするか(50で0.01なら0.5秒")]
    float hitPowerPercenage = 0.003f;
    public float HitPowerPercenage { get { return hitPowerPercenage; } }

}