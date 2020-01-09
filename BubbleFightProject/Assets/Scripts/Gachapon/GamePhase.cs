using UnityEngine;

[CreateAssetMenu(
    fileName = "GamePhase",
    order = 0)]
public class GamePhase : ScriptableObject
{
    [Header("フェーズ時間")]
    public float phaseTime = 60;
    [Header("玉を出す上限")]
    public int ballNumUpperLimit = 5;
    [Header("玉の出るスピード")]
    public float ballSpeed = 300;
    [Header("玉を出す間隔の上限")]
    public float ballGeneratIntervalUpperLimitTime = 0;
    [Header("玉を出す間隔の下限")]
    public float ballGeneratIntervalLowerLimitTime = 0;
}
