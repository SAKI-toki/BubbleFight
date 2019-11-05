using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーの種類選択の管理クラス
/// </summary>
public class PlayerTypeSelectManager : MonoBehaviour
{
    [SerializeField, Tooltip("カーソルのPrefab")]
    GameObject cursorPrefab = null;
    //カーソルリスト
    List<PlayerTypeSelectCorsorController> cursors = new List<PlayerTypeSelectCorsorController>();

    void Start()
    {
#if UNITY_EDITOR
        PlayerJoinManager.DebugSetPlayerJoinCount(1);
#endif
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (PlayerJoinManager.IsJoin(i))
            {
                //カーソルの生成
                var cursor = Instantiate(cursorPrefab).GetComponent<PlayerTypeSelectCorsorController>();
                cursor.SetPlayerNumber(i);
                cursors.Add(cursor);
            }
        }
    }

    void Update()
    {
        //カーソルの更新
        foreach (var cursor in cursors)
        {
            cursor.CursorUpdate();
        }

        //シーン遷移
        if (SwitchInput.GetButtonDown(0, SwitchButton.Pause) &&
            AlreadyAllPlayerSelect())
        {
            SceneManager.LoadScene("StageSelectScene");
        }
    }

    /// <summary>
    /// 全てのプレイヤーが種類を選択したかどうか
    /// </summary>
    bool AlreadyAllPlayerSelect()
    {
        foreach (var cursor in cursors)
        {
            //一人でもまだ選択していないならfalse
            if (!cursor.AlreadySelectType())
            {
                return false;
            }
        }
        return true;
    }

    public enum PlayerTypeEnum { Dog, None };
    static PlayerTypeEnum[] playerTypes = new PlayerTypeEnum[PlayerCount.MaxValue];
    /// <summary>
    /// プレイヤーの種類を取得
    /// </summary>
    public static PlayerTypeEnum GetPlayerType(int index)
    {
        return playerTypes[index];
    }
    /// <summary>
    /// プレイヤーの種類をセット
    /// </summary>
    public static void SetPlayerType(int index, PlayerTypeEnum playerType)
    {
        playerTypes[index] = playerType;
    }
}