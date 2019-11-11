using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (SwitchInput.GetButtonDown(i, SwitchButton.Ok))
            {
                JoinUnJoinExecute(i, true);
            }
            //非参加
            else if (SwitchInput.GetButtonDown(i, SwitchButton.Cancel))
            {
                JoinUnJoinExecute(i, false);
            }
        }

        //参加人数の更新
        UpdateJoinCount();

        //プレイ人数確定
        if (SwitchInput.GetButtonDown(0, SwitchButton.Pause) &&
            joinPlayerCount >= PlayerCount.MinValue)
        {
            //次のシーン
            SceneManager.LoadScene("PlayerTypeSelectScene");
        }
    }

    /// <summary>
    /// 参加、不参加の実行
    /// </summary>
    void JoinUnJoinExecute(int index, bool join)
    {
        if (join == IsJoin(index)) return;
        if (join)
        {
            joinControllers[index].Join();
        }
        else //if(!join)
        {
            joinControllers[index].UnJoin();
        }
        isJoins[index] = join;
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
    static int joinPlayerCount = 4;
    //参加かどうか
    static bool[] isJoins = { true, true, true, true, false, false, false, false };

    static public int GetJoinPlayerCount() { return joinPlayerCount; }
    static public bool IsJoin(int index) { return isJoins[index]; }

#if UNITY_EDITOR
    /// <summary>
    /// デバッグ用にどこからでもプレイヤーの人数をいじれるようにする
    /// </summary>
    static public void DebugSetPlayerJoinCount(int PlayerCount)
    {
        joinPlayerCount = PlayerCount;
        for (int i = 0; i < PlayerCount; ++i)
        {
            isJoins[i] = true;
        }
    }
    [SerializeField, Tooltip("デバッグ用のオンオフを実行するプレイヤー")]
    int debugPlayerNumber = 0;
    [SerializeField, Tooltip("デバッグ用のオンオフ")]
    bool debugOnOff = false;
    /// <summary>
    /// デバッグ用のオンオフの実行
    /// </summary>
    [ContextMenu("デバッグ用のオンオフの実行")]
    void DebugOnOff()
    {
        JoinUnJoinExecute(debugPlayerNumber, debugOnOff);
    }
#endif
}

/// <summary>
/// プレイヤーの参加に関する情報
/// </summary>
public static class PlayerCount
{
    public const int MinValue = 2;
    public const int MaxValue = 8;
}