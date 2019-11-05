using UnityEngine;

/// <summary>
/// プレイヤーのゲームへの参加の制御
/// </summary>
public class PlayerJoinController : MonoBehaviour
{
    /// <summary>
    /// 参加
    /// </summary>
    public void Join()
    {
        transform.localScale = Vector3.one * 1.5f;
    }

    /// <summary>
    /// 非参加
    /// </summary>
    public void UnJoin()
    {
        transform.localScale = Vector3.one;
    }
}