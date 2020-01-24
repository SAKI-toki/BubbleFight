using UnityEngine;

[CreateAssetMenu]
public class AlpacaStatus : PlayerTypeStatusScriptableObject
{
    /// <summary>
    /// アルパカの初期化
    /// </summary>
    public void AlpacaStatusInit(ref float movePower, ref float easyCurveWeight,
        ref float mass, ref float boostPower, ref float boostInterval, ref float brakePower)
    {
        movePower = Random.Range(minBallMovePower, maxBallMovePower);
        easyCurveWeight = Random.Range(minBallEasyCurveWeight, maxBallEasyCurveWeight);
        mass = Random.Range(minBallMass, maxBallMass);
        boostPower = Random.Range(minBallBoostPower, maxBallBoostPower);
        boostInterval = Random.Range(minBallBoostInterval, maxBallBoostInterval);
        brakePower = Random.Range(minBallBrakePower, maxBallBrakePower);
    }

    [SerializeField, Header("アルパカの場合は以下のパラメータを変更してください"), Tooltip("ボールでの移動時の力(最低)")]
    float minBallMovePower = 10.0f;
    [SerializeField, Tooltip("ボールでの移動時の力(最高)")]
    float maxBallMovePower = 10.0f;

    [SerializeField, Tooltip("ボールの曲がりやすくする重み(最低)"), Range(1, 2)]
    float minBallEasyCurveWeight = 1.0f;
    [SerializeField, Tooltip("ボールの曲がりやすくする重み(最高)"), Range(1, 2)]
    float maxBallEasyCurveWeight = 1.0f;

    [SerializeField, Tooltip("ボールの重さ(最低)")]
    float minBallMass = 1.0f;
    [SerializeField, Tooltip("ボールの重さ(最高)")]
    float maxBallMass = 1.0f;

    [SerializeField, Tooltip("ボールのブースト時の力(最低)")]
    float minBallBoostPower = 20.0f;
    [SerializeField, Tooltip("ボールのブースト時の力(最高)")]
    float maxBallBoostPower = 20.0f;

    [SerializeField, Tooltip("ボールのブーストを再使用できる間隔(最低)")]
    float minBallBoostInterval = 1.0f;
    [SerializeField, Tooltip("ボールのブーストを再使用できる間隔(最高)")]
    float maxBallBoostInterval = 1.0f;

    [SerializeField, Tooltip("ボールのブレーキの強さ(最低)"), Range(1, 10)]
    float minBallBrakePower = 1.0f;
    [SerializeField, Tooltip("ボールのブレーキの強さ(最高)"), Range(1, 10)]
    float maxBallBrakePower = 1.0f;
}
