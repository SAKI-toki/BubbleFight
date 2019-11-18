using UnityEngine;

public class AddPointUIGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("+のUIリスト")]
    GameObject[] PlusUIList = null;
    [SerializeField, Tooltip("-のUIリスト")]
    GameObject[] MinusUIList = null;



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

    void PlusPoint(int point, Camera renderCamera)
    {
        SetCamera(Instantiate(PlusUIList[point - 1]), renderCamera);
    }

    void MinusPoint(int point, Camera renderCamera)
    {
        SetCamera(Instantiate(MinusUIList[Mathf.Abs(point) - 1]), renderCamera);
    }

    void SetCamera(GameObject ui, Camera renderCamera)
    {
        var canvas = ui.GetComponent<Canvas>();
        canvas.worldCamera = renderCamera;
        canvas.planeDistance = renderCamera.nearClipPlane + 0.001f;
    }
}