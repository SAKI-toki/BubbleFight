using UnityEngine;
using System.Collections;

/// <summary>
/// リスポーンさせるオブジェクト
/// </summary>
public class RespawnBrokenObject : BrokenObject
{
    [SerializeField, Tooltip("リスポーンの間隔")]
    float respawnIntervalTime = 10.0f;

    protected override void Broken()
    {
        ObjectOnOff(false);
        StartCoroutine(RespawnCoroutine());
    }

    /// <summary>
    /// オブジェクトのオンオフ
    /// </summary>
    void ObjectOnOff(bool on)
    {
        foreach (var collider in GetComponents<Collider>()) collider.enabled = on;
        foreach (var renderer in GetComponents<Renderer>()) renderer.enabled = on;
    }

    /// <summary>
    /// リスポーンさせるコルーチン
    /// </summary>
    IEnumerator RespawnCoroutine()
    {
        float respawnTimeCount = 0.0f;
        while (respawnTimeCount < respawnIntervalTime)
        {
            respawnTimeCount += Time.deltaTime;
            yield return null;
        }
        ObjectOnOff(true);
        SetMaxHp();
        alreadyBroken = false;
    }
}