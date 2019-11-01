using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの参加かどうかを管理するクラス
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    [SerializeField, Tooltip("個々の参加の制御")]
    List<PlayerJoinController> joinControllers = new List<PlayerJoinController>();

    void Start()
    {
        Debug.Assert(PlayerCount.MaxValue == joinControllers.Count);
        ResetJoinInfo();
    }

    void Update()
    {
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            //参加
            if (SwitchInput.GetButtonDown(i, SwitchButton.Ok) && !IsJoin(i))
            {
                joinControllers[i].Join();
                isJoins[i] = true;
            }
            //非参加
            else if (SwitchInput.GetButtonDown(i, SwitchButton.Cancel) && IsJoin(i))
            {
                joinControllers[i].UnJoin();
                isJoins[i] = false;
            }
        }

        UpdateJoinCount();

        //プレイ人数確定
        if (SwitchInput.GetButtonDown(0, SwitchButton.Pause) &&
            joinPlayerCount >= PlayerCount.MinValue)
        {
            //次のシーン
        }
    }

    /// <summary>
    /// 参加人数の更新
    /// </summary>
    void UpdateJoinCount()
    {
        //参加人数
        joinPlayerCount = 0;
        foreach (var isJoin in isJoins)
        {
            if (isJoin) ++joinPlayerCount;
        }
    }

    /// <summary>
    /// 参加情報をリセット
    /// </summary>
    void ResetJoinInfo()
    {
        for (int i = 0; i < isJoins.Length; ++i) isJoins[i] = false;
        joinPlayerCount = 0;
    }

    //参加人数(別の場所でわざわざカウントしないようにする)
    static int joinPlayerCount = 0;
    //参加かどうか
    static bool[] isJoins = new bool[PlayerCount.MaxValue];

    static public int GetJoinPlayerCount() { return joinPlayerCount; }
    static public bool IsJoin(int index) { return isJoins[index]; }
}
