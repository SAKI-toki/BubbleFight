using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// フェードのポストプロセス
/// </summary>
public class FadePostprocess : Postprocess
{
    float percentValue = 0.0f;
    bool isFade = false;

    public bool IsFade { get { return this.isFade; } }

    /// <summary>
    /// 値のセット
    /// </summary>
    void SetValue(float value)
    {
        percentValue = Mathf.Clamp(value, 0.0f, 1.0f);
        material.SetFloat("_Percent", percentValue);
    }

    protected override void Initialize()
    {
        SetValue(1.0f);
    }

    /// <summary>
    /// フェードアウトのスタート
    /// </summary>
    public void StartFadeOut(string nextSceneName)
    {
        if (isFade) return;
        isFade = true;
        StartCoroutine(FadeOut(nextSceneName));
    }

    /// <summary>
    /// フェードインのスタート
    /// </summary>
    public void StartFadeIn()
    {
        if (isFade) return;
        isFade = true;
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    IEnumerator FadeOut(string nextSceneName)
    {
        float percent = 0.0f;
        while (percent < 1.0f)
        {
            percent += Time.unscaledDeltaTime / 2;
            SetValue(percent);
            yield return null;
        }
        isFade = false;
        SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    IEnumerator FadeIn()
    {
        float percent = 1.0f;
        while (percent > 0.0f)
        {
            percent -= Time.unscaledDeltaTime / 2;
            SetValue(percent);
            yield return null;
        }
        isFade = false;
    }
}
