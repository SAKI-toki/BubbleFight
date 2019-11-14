using UnityEngine;

/// <summary>
/// 最後にぶつかったプレイヤーをセットしておく
/// </summary>
static public class LastHitPlayerManager
{
    class LastHitPlayer
    {
        public int lastHitPlayerIndex = int.MaxValue;
        public float realTime = 0.0f;
    }

    //最後にぶつかったプレイヤーを格納
    static LastHitPlayer[] lastHitPlayers;

    /// <summary>
    /// 最後にぶつかったプレイヤーが有効かどうか
    /// </summary>
    static public bool IsValid(int index)
    {
        const float LastHitInterval = 5.0f;

        return (lastHitPlayers[index].lastHitPlayerIndex != int.MaxValue &&
            Time.realtimeSinceStartup - lastHitPlayers[index].realTime < LastHitInterval);
    }

    /// <summary>
    /// 最後にぶつかったプレイヤーを返す
    /// </summary>
    static public int GetLastHitPlayer(int index)
    {
        return lastHitPlayers[index].lastHitPlayerIndex;
    }

    /// <summary>
    /// 最後にぶつかったプレイヤーをセットする
    /// </summary>
    static public void SetLastHitPlayer(int index, int hitIndex)
    {
        if (index == int.MaxValue || hitIndex == int.MaxValue) return;
        lastHitPlayers[index].lastHitPlayerIndex = hitIndex;
        lastHitPlayers[index].realTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 最後にぶつかったプレイヤーを利用したらリセットする
    /// </summary>
    static public void UseLastHitPlayer(int index)
    {
        lastHitPlayers[index].lastHitPlayerIndex = int.MaxValue;
    }

    /// <summary>
    /// リセット
    /// </summary>
    static public void Reset()
    {
        lastHitPlayers = new LastHitPlayer[PlayerJoinManager.GetJoinPlayerCount()];
        for (int i = 0; i < lastHitPlayers.Length; ++i)
            lastHitPlayers[i] = new LastHitPlayer();
    }
}