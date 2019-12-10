using UnityEngine;

/// <summary>
/// ゲームの時間を管理するクラス
/// </summary>
public class GameTimeManager : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームの時間")]
    float playTime = 120.0f;

    float playTimeCount = 0.0f;

    void Start()
    {
        playTimeCount = playTime;
    }

    /// <summary>
    /// 時間の加算
    /// </summary>
    public void AddTime()
    {
        playTimeCount -= Time.deltaTime;
    }

    /// <summary>
    /// ゲーム中かどうか
    /// </summary>
    public bool IsPlayGame()
    {
        return playTimeCount > 0;
    }

    void OnGUI()
    {
        Rect rect = new Rect(0, 0, 100, 100);
        GUI.color = Color.red;
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        const float interval = 100.0f;
        for (int i = 0; i < PlayerJoinManager.GetJoinPlayerCount(); ++i)
        {
            GUI.Label(rect, "Player" + (i + 1).ToString() + " : " + PointManager.GetPoint(i) + "P");
            rect.x += interval;
        }
        if (!IsPlayGame())
        {
            GUI.Label(rect, "END GAME Retry:+/-(1P)", style);
        }
        else
        {
            GUI.Label(rect, ((int)playTimeCount).ToString(), style);
        }
    }
}