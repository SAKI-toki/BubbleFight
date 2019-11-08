using UnityEngine;

/// <summary>
/// ゲームの時間を管理するクラス
/// </summary>
public class GameTimeManager : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームの時間")]
    float playTime = 120.0f;

    float playTimeCount = 0.0f;

    /// <summary>
    /// 時間の加算
    /// </summary>
    public void AddTime()
    {
        playTimeCount += Time.deltaTime;
    }

    /// <summary>
    /// ゲーム中かどうか
    /// </summary>
    public bool IsPlayGame()
    {
        return playTimeCount < playTime;
    }

    void OnGUI()
    {
        Rect rect = new Rect(0, 0, 100, 100);
        const float interval = 100.0f;
        for (int i = 0; i < PlayerJoinManager.GetJoinPlayerCount(); ++i)
        {
            GUI.Label(rect, "Player" + i.ToString() + " : " + PointManager.GetPoint(i) + "P");
            rect.x += interval;
        }
        if (!IsPlayGame())
        {
            GUI.Label(rect, "END GAME");
        }
    }
}