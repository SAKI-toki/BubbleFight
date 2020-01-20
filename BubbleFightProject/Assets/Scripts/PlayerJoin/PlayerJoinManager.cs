using UnityEngine;

/// <summary>
/// プレイヤーの参加かどうかを管理するクラス
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    //参加かどうか
    static bool[] isJoins = { true, true, true, true };

    static public bool IsJoin(int index) { return isJoins[index]; }

    static public void SetJoinInfo(int index, bool isJoin)
    {
        isJoins[index] = isJoin;
    }

}

/// <summary>
/// プレイヤーの参加に関する情報
/// </summary>
public static class PlayerCount
{
    public const int MinValue = 2;
    public const int MaxValue = 4;
}
