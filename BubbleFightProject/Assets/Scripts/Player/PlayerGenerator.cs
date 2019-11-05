using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    /// <summary>
    /// プレイヤーを生成しリストで返す
    /// </summary>
    public List<GameObject> GenerateAllJoinPlayerReturnList()
    {
        List<GameObject> playerList = new List<GameObject>();
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (PlayerJoinManager.IsJoin(i))
            {
                playerList.Add(GeneratePlayer(i));
            }
        }
        return playerList;
    }

    /// <summary>
    /// プレイヤーを生成し配列で返す
    /// </summary>
    public GameObject[] GenerateAllJoinPlayerReturnArray()
    {
        GameObject[] playerList = new GameObject[PlayerCount.MaxValue];
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (PlayerJoinManager.IsJoin(i))
            {
                playerList[i] = GeneratePlayer(i);
            }
        }
        return playerList;
    }

    /// <summary>
    /// プレイヤーを生成
    /// </summary>
    public GameObject GeneratePlayer(int index)
    {
        return null;
    }
}