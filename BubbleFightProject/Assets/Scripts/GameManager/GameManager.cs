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
    AudioSource aud;

    void Start()
    {
        PointManager.Reset();
        Time.timeScale = 0.0f;
        BgmManager.GetInstance().Stop();
        aud = GetComponent<AudioSource>();
        StartCoroutine(Countdown());
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

    [SerializeField]
    GameObject[] countDownImage = null;

    IEnumerator Countdown()
    {
        aud.Play();
        for (int count = countDownImage.Length - 1; count >= 0; count--)
        {
            //表示
            countDownImage[count].SetActive(true);
            //1秒待つ
            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            //消す
            countDownImage[count].SetActive(false);
        }
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(1.0f);
        BgmManager.GetInstance().Play(BgmEnum.Game);
    }
}
