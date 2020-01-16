using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームを管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームの時間を管理するクラス")]
    GameTimeManager gameTimeManager = null;

    bool endFlag = false;

    void Start()
    {
        PointManager.Reset();
        BgmManager.GetInstance().Play(BgmEnum.Game);
    }

    void Update()
    {
        gameTimeManager.AddTime();
        if (!gameTimeManager.IsPlayGame() && !endFlag)
        {
            endFlag = true;
            StartCoroutine(AllFadeCamera());
            PointManager.PointLock();
        }
    }

    /// <summary>
    /// 全てのカメラのフェードをしシーン遷移
    /// </summary>
    IEnumerator AllFadeCamera()
    {
        var postprocess = Camera.main.GetComponent<FadePostprocess>();
        float percent = 0.0f;
        while (percent < 1.0f)
        {
            percent += Time.deltaTime / 2;
            postprocess.SetValue(percent);
            yield return null;
        }
        SceneManager.LoadScene("ResultScene");
    }
}
