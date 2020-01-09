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

        int joinPlayerCount = 0;

        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (IsJoin(i)) ++joinPlayerCount;
        }

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
    /// 参加情報をリセット
    /// </summary>
    void ResetJoinInfo()
    {
        for (int i = 0; i < isJoins.Length; ++i) isJoins[i] = false;
    }

    //参加かどうか
    static bool[] isJoins = { true, true, false, false };

    static public bool IsJoin(int index) { return isJoins[index]; }
}

/// <summary>
/// プレイヤーの参加に関する情報
/// </summary>
public static class PlayerCount
{
    public const int MinValue = 2;
    public const int MaxValue = 4;
}