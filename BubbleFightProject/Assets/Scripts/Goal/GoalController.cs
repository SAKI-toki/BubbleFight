using UnityEngine;

/// <summary>
/// ゴールの制御クラス
/// </summary>
public class GoalController : MonoBehaviour
{
    [SerializeField, Tooltip("ゴールの番号")]
    int goalNumber = 0;

    /// <summary>
    /// ゴールの番号を取得
    /// </summary>
    public int GetGoalNumber()
    {
        return goalNumber;
    }
}