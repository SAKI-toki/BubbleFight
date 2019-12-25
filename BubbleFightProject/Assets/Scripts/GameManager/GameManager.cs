using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームを管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームの時間を管理するクラス")]
    GameTimeManager gameTimeManager = null;
    [SerializeField, Tooltip("ポイントを取得したUIの生成クラス")]
    AddPointUIGenerator addPointUIGenerator = null;
    [SerializeField, Tooltip("フェードシェーダー")]
    Shader fadeShader = null;

    bool endFlag = false;

    void Start()
    {
        PointManager.Reset();
        LastHitPlayerManager.Reset();
        PointManager.AddPointFunction = delegate (int index, int point)
        {
            addPointUIGenerator.AddPoint(index, point);
        };
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
        Postprocess postprocess = Camera.main.GetComponent<Postprocess>();
        postprocess.SetMaterial(new Material(fadeShader));
        float percent = 0.0f;
        while (percent < 1.0f)
        {
            percent += Time.deltaTime / 2;
            postprocess.ApplyMaterialFunction(delegate (Material material) { material.SetFloat("_Percent", percent); });
            yield return null;
        }
        SceneManager.LoadScene("ResultScene");
    }
}