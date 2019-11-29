using UnityEngine;

/// <summary>
/// ぷストプロセス
/// </summary>
public class Postprocess : MonoBehaviour
{
    public delegate void ApplyMaterialFunctionType(Material applyMaterial);
    Material material = null;

    /// <summary>
    /// マテリアルのセット
    /// </summary>
    public void SetMaterial(Material postprocessMaterial)
    {
        material = postprocessMaterial;
    }

    /// <summary>
    /// マテリアルに適用させる
    /// </summary>
    public void ApplyMaterialFunction(ApplyMaterialFunctionType func)
    {
        func(material);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material)
        {
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}