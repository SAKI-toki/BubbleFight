using UnityEngine;

public class MaterialFlash : MonoBehaviour
{
    [SerializeField, Tooltip("色")]
    Color flashColor = Color.white;

    Color defaultColor = Color.white;

    Material material = null;

    float intervalTime = 0.0f;

    float timeCount = 0.0f;
    //スタートしたかどうか
    bool isStart = false;

    void Update()
    {
        if (!isStart) return;

        timeCount += Time.deltaTime;
        if (timeCount > intervalTime)
        {
            timeCount = 0.0f;
            Flash();
        }
    }

    /// <summary>
    /// 間隔のセット
    /// </summary>
    public void SetInterval(float interval)
    {
        intervalTime = interval;
    }

    /// <summary>
    /// マテリアルのセット
    /// </summary>
    public void SetMaterial(Material mat)
    {
        material = mat;
        defaultColor = material.color;
    }

    /// <summary>
    /// 点滅の開始
    /// </summary>
    public void FlashStart()
    {
        if (isStart) return;
        isStart = true;
        material.color = defaultColor;
    }

    /// <summary>
    /// 点滅の終了
    /// </summary>
    public void FlashEnd()
    {
        if (!isStart) return;
        isStart = false;
        material.color = defaultColor;
    }

    bool on = false;

    /// <summary>
    /// 点滅
    /// </summary>
    void Flash()
    {
        on = !on;
        if (on)
        {
            material.color = flashColor;
        }
        else
        {
            material.color = defaultColor;
        }
    }
}