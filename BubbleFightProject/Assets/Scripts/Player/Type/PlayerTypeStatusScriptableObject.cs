using UnityEngine;


/// <summary>
/// プレイヤーのステータスをタイプごとに設定する
/// </summary>
[CreateAssetMenu]
public class PlayerTypeStatusScriptableObject : ScriptableObject
{
    [SerializeField, Tooltip("プレイヤーのタイプ")]
    PlayerType type = PlayerType.None;
    public PlayerType Type { get { return type; } }

    [SerializeField, Tooltip("歩く速度")]
    float walkSpeed = 10.0f;
    public float WalkSpeed { get { return walkSpeed; } }

    [SerializeField, Tooltip("ボールでの移動時の力")]
    float ballMovePower = 10.0f;
    public float BallMovePower { get { return ballMovePower; } }

    [SerializeField, Tooltip("ボールに付ける物理マテリアル")]
    PhysicMaterial ballPhysicalMaterial = null;
    public PhysicMaterial BallPhysicalMaterial { get { return ballPhysicalMaterial; } }

    [SerializeField, Tooltip("ボールの曲がりやすくする重み"), Range(1, 2)]
    float ballEasyCurveWeight = 1.0f;
    public float BallEasyCurveWeight { get { return ballEasyCurveWeight; } }

    [SerializeField, Tooltip("ボールの重さ")]
    float ballMass = 1.0f;
    public float BallMass { get { return ballMass; } }
    [SerializeField, Tooltip("ボールのブースト時の力")]
    float ballBoostPower = 20.0f;
    public float BallBoostPower { get { return ballBoostPower; } }
    [SerializeField, Tooltip("ボールのブーストを再使用できる間隔")]
    float ballBoostInterval = 1.0f;
    public float BallBoostInterval { get { return ballBoostInterval; } }

}