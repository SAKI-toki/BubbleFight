using UnityEngine;
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

    void Start()
    {
        PointManager.Reset();
        LastHitPlayerManager.Reset();
        CameraManager.Reset();
        PointManager.AddPointFunction = delegate (int index, int point)
        {
            addPointUIGenerator.AddPoint(index, point);
        };
    }

    void Update()
    {
        gameTimeManager.AddTime();
        if (!gameTimeManager.IsPlayGame()) PointManager.PointLock();
        if (SwitchInput.GetButtonDown(0, SwitchButton.Pause))
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}