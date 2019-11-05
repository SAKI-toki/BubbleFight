using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// ??????????????????????
/// </summary>
public class PlayerTypeSelectManager : MonoBehaviour
{
    [SerializeField, Tooltip("????Prefab")]
    GameObject cursorPrefab = null;
    //???????
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
                //???????
                var cursor = Instantiate(cursorPrefab).GetComponent<PlayerTypeSelectCorsorController>();
                cursor.SetPlayerNumber(i);
                cursors.Add(cursor);
            }
        }
    }

    void Update()
    {
        //????????
        foreach (var cursor in cursors)
        {
            cursor.CursorUpdate();
        }

        //?????
        if (SwitchInput.GetButtonDown(0, SwitchButton.Pause) &&
            AlreadyAllPlayerSelect())
        {
            SceneManager.LoadScene("StageSelectScene");
        }
    }

    /// <summary>
    /// ?????????????????
    /// </summary>
    bool AlreadyAllPlayerSelect()
    {
        foreach (var cursor in cursors)
        {
            //??????????????false
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
    /// ???????????
    /// </summary>
    public static PlayerTypeEnum GetPlayerType(int index)
    {
        return playerTypes[index];
    }
    /// <summary>
    /// ????????????
    /// </summary>
    public static void SetPlayerType(int index, PlayerTypeEnum playerType)
    {
        playerTypes[index] = playerType;
    }
}