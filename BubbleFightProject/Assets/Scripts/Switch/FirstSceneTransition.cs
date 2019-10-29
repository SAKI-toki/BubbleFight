using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// switchではUnityのロゴが出ているときもシーンが進行してしまうので
/// 最初のシーンで待機する
/// </summary>
public class FirstSceneTransition : MonoBehaviour
{
    const string TitleSceneName = "TitleScene";
    const float WaitTime = 3.0f;
    void Start()
    {
        StartCoroutine(LoadStartScene());
    }

    /// <summary>
    /// 最初のシーンに指定した秒数待機して遷移する
    /// </summary>
    IEnumerator LoadStartScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(TitleSceneName);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(WaitTime);
        async.allowSceneActivation = true;
    }
}
