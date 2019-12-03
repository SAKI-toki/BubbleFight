using UnityEngine;

/// <summary>
/// フレームレートを表示
/// </summary>
public class FpsOnGUI : MonoBehaviour
{
    [SerializeField, Tooltip("更新頻度")]
    float updateFrequencyTime = 1.0f;
    //時間を計測
    float timeCount = 0.0f;
    //フレームをカウント
    int frameCount = 0;
    //フレームレート
    float framesPerSecond = 60.0f;

    void Update()
    {
        ++frameCount;
        timeCount += Time.deltaTime;
        if (timeCount >= updateFrequencyTime)
        {
            framesPerSecond = frameCount / timeCount;
            timeCount = 0;
            frameCount = 0;
        }
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        GUI.Label(new Rect(Screen.width - 200, Screen.height - 30, 200, 30), "FPS : " + framesPerSecond.ToString("F"), style);
    }
}