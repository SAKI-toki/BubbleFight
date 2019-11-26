using UnityEngine;

public class PostEffect : MonoBehaviour
{
    int cameraIndex = 0;
    [SerializeField, Tooltip("ポストエフェクトシェーダー")]
    Shader postEffectShader = null;
    Material postEffectMaterial = null;

    void Start()
    {
        postEffectMaterial = new Material(postEffectShader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        var color = SwitchColor.GetColor(cameraIndex);
        postEffectMaterial.SetColor("_Right", color.right);
        postEffectMaterial.SetColor("_Left", color.left);
        Graphics.Blit(src, dest, postEffectMaterial);
    }

    public void SetIndex(int index)
    {
        cameraIndex = index;
    }
}