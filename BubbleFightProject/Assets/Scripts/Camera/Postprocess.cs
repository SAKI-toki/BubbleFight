using UnityEngine;

/// <summary>
/// ポストプロセス
/// </summary>
public abstract class Postprocess : MonoBehaviour
{
    [SerializeField, Tooltip("シェーダー")]
    Shader shader = null;

    protected Material material = null;

    void Awake()
    {
        material = new Material(shader);
        Initialize();
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

    protected abstract void Initialize();
}
