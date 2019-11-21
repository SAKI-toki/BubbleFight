using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーのタイプを管理するクラス
/// </summary>
public class PlayerTypeManager : Singleton<PlayerTypeManager>
{
    [SerializeField, Tooltip("生成するプレイヤーリスト")]
    GameObject[] playerList = null;

    Dictionary<PlayerType, GameObject> dictionaryPlayerByType = new Dictionary<PlayerType, GameObject>();

    static PlayerType[] playerTypes = new PlayerType[PlayerCount.MaxValue];

    public override void MyStart()
    {
        foreach (var player in playerList)
        {
            var playerController = player.GetComponent<PlayerController>();
            var playerType = playerController.GetPlayerType();
            if (dictionaryPlayerByType.ContainsKey(playerType))
            {
                Debug.LogError("同じタイプのプレイヤーを入れています");
            }
            dictionaryPlayerByType.Add(playerType, player);
        }
    }

    /// <summary>
    /// プレイヤーのタイプをセット
    /// </summary>
    public void SetPlayerType(int index, PlayerType playerType)
    {
        playerTypes[index] = playerType;
    }

    /// <summary>
    /// プレイヤーを生成して返す
    /// </summary>
    public GameObject GeneratePlayer(int index)
    {
        return Instantiate(dictionaryPlayerByType[playerTypes[index]]);
    }
}

/// <summary>
/// プレイヤーのタイプ
/// </summary>
public enum PlayerType
{
    Alpaca,
    Bear,
    Cat,
    Chick,
    Chicken,
    Cow,
    Deer,
    Dog,
    Elephant,
    Hippo,
    Horse,
    Lion,
    Rabbit
}