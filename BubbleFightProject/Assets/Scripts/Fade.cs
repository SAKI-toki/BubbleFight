using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    static public Fade instance = null;
    [SerializeField]
    Image fadeImage = null;
    bool isFade = false;

    public bool IsFade { get { return this.isFade; } }

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 値のセット
    /// </summary>
    void SetValue(float value)
    {
        var col = Color.black;
        col.a = Mathf.Clamp(value, 0.0f, 1.0f);
        fadeImage.color = col;
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
        SetValue(percent);
        yield return null;
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
        SetValue(percent);
        yield return null;
        while (percent > 0.0f)
        {
            percent -= Time.unscaledDeltaTime / 2;
            SetValue(percent);
            yield return null;
        }
        isFade = false;
    }
}
