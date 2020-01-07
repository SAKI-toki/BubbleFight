using UnityEngine;

/// <summary>
/// ゲームの時間を管理するクラス
/// </summary>
public class GameTimeManager : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームの時間")]
    float playTime = 120.0f;

    float playTimeCount = 0.0f;

    [SerializeField]
    UnityEngine.UI.Text pointText = null, timeText = null;


    void Start()
    {
        playTimeCount = playTime;
        UpdatePointText();
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
        if (!IsPlayGame()) return;
        UpdatePointText();
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

    string nextPointText;
    /// <summary>
    /// ポイントテキストの更新
    /// </summary>
    void UpdatePointText()
    {
        nextPointText = "";
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (!PlayerJoinManager.IsJoin(i)) continue;

            nextPointText += "Player" + (i + 1).ToString() + " : " +
             PointManager.GetPoint(i) + "P  ";
        }

        if (nextPointText != pointText.text)
        {
            pointText.text = nextPointText;
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