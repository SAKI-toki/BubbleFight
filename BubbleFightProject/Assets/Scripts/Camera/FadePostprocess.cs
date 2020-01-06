using UnityEngine;

/// <summary>
/// フェードのポストプロセス
/// </summary>
public class FadePostprocess : Postprocess
{
    float percentValue = 0.0f;

    /// <summary>
    /// 値のセット
    /// </summary>
    public void SetValue(float value)
    {
        percentValue = Mathf.Clamp(value, 0.0f, 1.0f);
        material.SetFloat("_Percent", percentValue);
    }

    protected override void Initialize()
    {
        SetValue(0.0f);
    }
}