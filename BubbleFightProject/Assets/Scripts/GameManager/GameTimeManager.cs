using UnityEngine;

/// <summary>
/// ゲームの時間を管理するクラス
/// </summary>
public class GameTimeManager : MonoBehaviour
{
    float playTime = 180.0f;

    float playTimeCount = 0.0f;

    [SerializeField]
    UnityEngine.UI.Text timeText = null;


    void Start()
    {
        playTimeCount = playTime;
        UpdateTimeText();
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

    private void Update()
    {
        if (Time.timeScale == 0.0f || Fade.instance.IsFade) return;
        if (!IsPlayGame()) return;
        UpdateTimeText();
        int alivePlayerCount = 0;
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (!PlayerJoinManager.IsJoin(i)) continue;
            if (PointManager.GetPoint(i) != 0) ++alivePlayerCount;
        }
        if (alivePlayerCount <= 1)
        {
            playTimeCount = 0.0f;
        }
    }

    string nextTimeText;
    /// <summary>
    /// 時間テキストの更新
    /// </summary>
    void UpdateTimeText()
    {
        nextTimeText = "";
        if (playTimeCount > 0)
        {
            nextTimeText = "Time : " + ((int)playTimeCount).ToString();
        }
        else
        {
            nextTimeText = "END";
        }

        if (nextTimeText != timeText.text)
        {
            timeText.text = nextTimeText;
        }
    }
}
