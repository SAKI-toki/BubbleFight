using UnityEngine;

/// <summary>
/// フレームレートを表示
/// </summary>
public class FpsUi : MonoBehaviour
{
    [SerializeField, Tooltip("更新頻度")]
    float updateFrequencyTime = 1.0f;
    //時間を計測
    float timeCount = 0.0f;
    //フレームをカウント
    int frameCount = 0;

    UnityEngine.UI.Text fpsText = null;

    void Start()
    {
        fpsText = GetComponent<UnityEngine.UI.Text>();
        UpdateText(60.0f);
    }

    void Update()
    {
        ++frameCount;
        timeCount += Time.deltaTime;
        if (timeCount >= updateFrequencyTime)
        {
            UpdateText(frameCount / timeCount);
            timeCount = 0;
            frameCount = 0;
        }
    }

    /// <summary>
    /// テキストの更新
    /// </summary>
    void UpdateText(float frameRate)
    {
        fpsText.text = "FPS : " + frameRate.ToString("F");
    }
}