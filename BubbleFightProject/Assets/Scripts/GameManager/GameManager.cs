using System.Collections;
using UnityEngine;

/// <summary>
/// ゲームを管理するクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームの時間を管理するクラス")]
    GameTimeManager gameTimeManager = null;

    bool endFlag = false;
    AudioSource aud;

    FadePostprocess fade = null;

    void Start()
    {
        PointManager.Reset();
        Time.timeScale = 0.0f;
        BgmManager.GetInstance().Stop();
        aud = GetComponent<AudioSource>();
        fade = Camera.main.GetComponent<FadePostprocess>();
        fade.StartFadeIn();
        StartCoroutine(Countdown());
    }

    void Update()
    {
        if (Time.timeScale == 0.0f) return;
        gameTimeManager.AddTime();
        if (!gameTimeManager.IsPlayGame() && !endFlag)
        {
            endFlag = true;
            fade.StartFadeOut("ResultScene");
            PointManager.PointLock();
        }
    }

    [SerializeField]
    GameObject[] countDownImage = null;

    IEnumerator Countdown()
    {
        while (fade.IsFade) yield return null;
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
