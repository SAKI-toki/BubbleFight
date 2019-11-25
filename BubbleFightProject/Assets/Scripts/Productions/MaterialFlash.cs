using UnityEngine;

public class MaterialFlash : MonoBehaviour
{
    [SerializeField, Tooltip("色")]
    Color flashColor = Color.white;
    [SerializeField, Tooltip("点滅の間隔")]
    float intervalTime = 0.0f;
    [SerializeField, Header("どちらかしか機能しない(MeshRenderer優先)"), Tooltip("点滅させるメッシュ")]
    MeshRenderer meshRenderer = null;
    [SerializeField, Tooltip("点滅させるスキンメッシュ")]
    SkinnedMeshRenderer skinMeshRenderer = null;

    Color defaultColor = Color.white;

    Material material = null;

    float timeCount = 0.0f;
    //スタートしたかどうか
    bool isStart = false;

    void Start()
    {
        if (meshRenderer && meshRenderer.material) material = meshRenderer.material;
        else if (skinMeshRenderer && skinMeshRenderer.material) material = skinMeshRenderer.material;
    }

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
    /// 点滅の色をセット
    /// </summary>
    public void SetFlashColor(Color color)
    {
        flashColor = color;
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
        if (!material) return;
        material.color = defaultColor;
    }

    /// <summary>
    /// 点滅の終了
    /// </summary>
    public void FlashEnd()
    {
        if (!isStart) return;
        isStart = false;
        if (!material) return;
        material.color = defaultColor;
    }

    bool on = false;

    /// <summary>
    /// 点滅
    /// </summary>
    void Flash()
    {
        if (!material) return;
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