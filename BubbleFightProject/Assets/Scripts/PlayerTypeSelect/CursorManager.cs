using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// カーソルの管理
/// </summary>
public class CursorManager : MonoBehaviour
{
    [SerializeField, Tooltip("カーソル")]
    GameObject cursorPrefab = null;
    //カーソルリスト
    List<CursorController> cursorControllers = new List<CursorController>();

    void Start()
    {
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (PlayerJoinManager.IsJoin(i))
            {
                var cursorController = Instantiate(cursorPrefab).GetComponent<CursorController>();
                cursorController.SetPlayerNumber(i);
                cursorControllers.Add(cursorController);
            }
        }
    }

    void Update()
    {
        int notSelectCount = 0;
        foreach (var cursor in cursorControllers)
        {
            cursor.CursorUpdate();
            if (!cursor.AlreadySelectType())
            {
                ++notSelectCount;
            }
        }
        if (notSelectCount == 0)
        {
            if (SwitchInput.GetButtonDown(0, SwitchButton.Pause))
            {
                //シーン遷移
                UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
            }
        }
    }
}