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
        prevPonits = new int[PlayerJoinManager.GetJoinPlayerCount()];
        prevTime = (int)playTimeCount;
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

    int[] prevPonits = null;
    int prevTime = 0;

    private void Update()
    {
        for (int i = 0; i < PlayerJoinManager.GetJoinPlayerCount(); ++i)
        {
            if (prevPonits[i] != PointManager.GetPoint(i))
            {
                UpdatePointText();
                break;
            }
        }
        if (prevTime != (int)playTimeCount)
            UpdateTimeText();
    }
    void UpdatePointText()
    {
        pointText.text = "";
        for (int i = 0; i < PlayerJoinManager.GetJoinPlayerCount(); ++i)
        {
            prevPonits[i] = PointManager.GetPoint(i);
            pointText.text += "Player" + (i + 1).ToString() + " : " + prevPonits[i] + "P  ";
        }
    }

    void UpdateTimeText()
    {
        prevTime = (int)playTimeCount;
        if (prevTime > 0)
        {
            timeText.text = "Time : " + prevTime.ToString();
        }
        else if (timeText.text[0] != 'E')
        {
            timeText.text = "END";
        }
    }
}