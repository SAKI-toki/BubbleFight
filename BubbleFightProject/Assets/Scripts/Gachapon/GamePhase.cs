using UnityEngine;

[CreateAssetMenu(
    fileName = "GamePhase",
    order = 0)]
public class GamePhase : ScriptableObject
{
    [Header("フェーズ時間(s)")]
    public float phaseTime = 60;
    [Header("玉を出す上限")]
    public int ballNumUpperLimit = 5;
    [Header("玉の出るスピード")]
    public float ballSpeed = 300;
    [Header("玉を出す間隔の上限(s)")]
    public float ballGeneratIntervalUpperLimitTime = 0;
    [Header("玉を出す間隔の下限(s)")]
    public float ballGeneratIntervalLowerLimitTime = 0;
    public const int bonusBallFrequencyDenominator = 3;
    [Header("ボーナスボールの出現頻度(x/3)"), Range(0, bonusBallFrequencyDenominator)]
    public int bonusBallFrequency = 0;
}
