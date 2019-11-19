using UnityEngine;

/// <summary>
/// ポイントの追加のUIの生成クラス
/// </summary>
public class AddPointUIGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("+のUIリスト")]
    GameObject[] PlusUIList = null;
    [SerializeField, Tooltip("-のUIリスト")]
    GameObject[] MinusUIList = null;

    /// <summary>
    /// 外部から呼び出す生成関数
    /// </summary>
    public void AddPoint(int index, int point)
    {
        if (point > 0)
        {
            PlusPoint(point, CameraManager.GetCamera(index));
        }
        else
        {
            MinusPoint(point, CameraManager.GetCamera(index));
        }
    }

    /// <summary>
    /// プラスの呼び出し
    /// </summary>
    void PlusPoint(int point, Camera renderCamera)
    {
        SetCamera(Instantiate(PlusUIList[point - 1]), renderCamera);
    }

    /// <summary>
    /// マイナスの呼び出し
    /// </summary>
    void MinusPoint(int point, Camera renderCamera)
    {
        SetCamera(Instantiate(MinusUIList[Mathf.Abs(point) - 1]), renderCamera);
    }

    /// <summary>
    /// カメラのセット
    /// </summary>
    void SetCamera(GameObject ui, Camera renderCamera)
    {
        var canvas = ui.GetComponent<Canvas>();
        canvas.worldCamera = renderCamera;
        canvas.planeDistance = renderCamera.nearClipPlane + 0.001f;
    }
}