using UnityEngine;

/// <summary>
/// プレイヤーのステータスをタイプごとに設定する
/// </summary>
[CreateAssetMenu]
public class PlayerTypeStatusScriptableObject : ScriptableObject
{
    [SerializeField, Tooltip("プレイヤーのタイプ")]
    protected PlayerType type = PlayerType.None;
    public PlayerType Type { get { return type; } }

    [SerializeField, Tooltip("ボールでの移動時の力")]
    protected float ballMovePower = 10.0f;
    public float BallMovePower { get { return ballMovePower; } }

    [SerializeField, Tooltip("ボールに付ける物理マテリアル")]
    protected PhysicMaterial ballPhysicalMaterial = null;
    public PhysicMaterial BallPhysicalMaterial { get { return ballPhysicalMaterial; } }

    [SerializeField, Tooltip("ボールの曲がりやすくする重み"), Range(1, 2)]
    protected float ballEasyCurveWeight = 1.0f;
    public float BallEasyCurveWeight { get { return ballEasyCurveWeight; } }

    [SerializeField, Tooltip("ボールの重さ")]
    protected float ballMass = 1.0f;
    public float BallMass { get { return ballMass; } }

    [SerializeField, Tooltip("ボールのブースト時の力")]
    protected float ballBoostPower = 20.0f;
    public float BallBoostPower { get { return ballBoostPower; } }

    [SerializeField, Tooltip("ボールのブーストを再使用できる間隔")]
    protected float ballBoostInterval = 1.0f;
    public float BallBoostInterval { get { return ballBoostInterval; } }

    [SerializeField, Tooltip("ボールのブレーキの強さ"), Range(1, 10)]
    protected float ballBrakePower = 1.0f;
    public float BallBrakePower { get { return ballBrakePower; } }

}
