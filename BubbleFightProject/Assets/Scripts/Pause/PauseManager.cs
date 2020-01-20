using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ポーズの管理クラス
/// </summary>
public class PauseManager : MonoBehaviour
{
    [SerializeField]
    Image blackBackImage = null;

    Fade fade = null;

    bool isTranslation = false;

    bool isPause = false;

    [SerializeField]
    PauseItem[] pauseItems = null;

    PauseItem currentItem = null;

    Vector3 defaultScale = Vector3.zero;
    float timeCount = 0.0f;

    const float SizeRange = 0.2f;

    void Start()
    {
        SetBackAlpha(0.0f);
        SetImageAlpha(0.0f);
        fade = Fade.instance;
        defaultScale = pauseItems[0].transform.localScale;
        pauseItems[0].SetFuntion(delegate
        {
            StartCoroutine(ReturnGame());
        });
        pauseItems[1].SetFuntion(delegate
        {
            Time.timeScale = 1.0f;
            fade.StartFadeOut("CharacterSelectScene");
        });
        pauseItems[2].SetFuntion(delegate
        {
            Time.timeScale = 1.0f;
            fade.StartFadeOut("TitleScene");
        });
    }

    void Update()
    {
        if (fade.IsFade) return;
        if (isTranslation) return;
        if (isPause)
        {
            if (SwitchInput.GetButtonDown(0, SwitchButton.Pause))
            {
                StartCoroutine(ReturnGame());
            }
            else
            {
                PauseItem nextItem = null;
                if (SwitchInput.GetButtonDown(0, SwitchButton.Ok))
                {
                    isPause = false;
                    currentItem.transform.localScale = Vector3.Scale(defaultScale
                    , new Vector3(1 + SizeRange, 1 + SizeRange, 1.0f));
                    currentItem.Execute();
                }
                else if (SwitchInput.GetButtonDown(0, SwitchButton.SelectUp))
                {
                    nextItem = currentItem.up;
                }
                else if (SwitchInput.GetButtonDown(0, SwitchButton.SelectDown))
                {
                    nextItem = currentItem.down;
                }
                else if (SwitchInput.GetButtonDown(0, SwitchButton.SelectRight))
                {
                    nextItem = currentItem.right;
                }
                else if (SwitchInput.GetButtonDown(0, SwitchButton.SelectLeft))
                {
                    nextItem = currentItem.left;
                }
                if (nextItem != null)
                {
                    currentItem.transform.localScale = defaultScale;
                    currentItem = nextItem;
                    timeCount = 0.0f;
                }
                float addScale = (Mathf.Sin(timeCount) + 1) / 2;
                float scale = 1 + addScale * SizeRange;
                currentItem.transform.localScale = Vector3.Scale(defaultScale
                    , new Vector3(scale, scale, 1.0f));
                timeCount += Time.unscaledDeltaTime * 2.0f;
            }
        }
        else
        {
            if (Time.timeScale == 0.0f) return;
            if (SwitchInput.GetButtonDown(0, SwitchButton.Pause))
            {
                currentItem = pauseItems[0];
                foreach (var pauseItem in pauseItems) pauseItem.transform.localScale = defaultScale;
                StartCoroutine(ToPause());
            }
        }
    }

    void SetBackAlpha(float alpha)
    {
        blackBackImage.color = new Color(0, 0, 0, Mathf.Clamp(alpha, 0.0f, 1.0f));
    }
    void SetImageAlpha(float alpha)
    {
        foreach (var image in GetComponentsInChildren<Image>())
        {
            if (image != blackBackImage)
            {
                var col = image.color;
                col.a = alpha;
                image.color = col;
            }
        }
    }

    IEnumerator ToPause()
    {
        isTranslation = true;
        isPause = true;
        Time.timeScale = 0.0f;
        float percent = 0.0f;
        while (percent < 0.5f)
        {
            SetBackAlpha(percent);
            SetImageAlpha(percent * 2);
            percent += Time.unscaledDeltaTime;
            yield return null;
        }
        SetBackAlpha(0.5f);
        SetImageAlpha(1.0f);
        isTranslation = false;
    }
    IEnumerator ReturnGame()
    {
        isTranslation = true;
        isPause = false;
        float percent = 0.5f;
        while (percent > 0.0f)
        {
            SetBackAlpha(percent);
            SetImageAlpha(percent * 2);
            percent -= Time.unscaledDeltaTime;
            yield return null;
        }
        SetBackAlpha(0.0f);
        SetImageAlpha(0.0f);
        Time.timeScale = 1.0f;
        isTranslation = false;
    }
}
