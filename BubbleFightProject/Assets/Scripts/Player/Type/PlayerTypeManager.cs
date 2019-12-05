using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーのタイプを管理するクラス
/// </summary>
public class PlayerTypeManager : Singleton<PlayerTypeManager>
{
    /// <summary>
    /// プレイヤーのタイプごとの情報
    /// </summary>
    [System.Serializable]
    class PlayerTypeInfo
    {
        public GameObject gamePlayer = null;
        public GameObject stageVotingPlayer = null;
        public PlayerTypeStatusScriptableObject statusObject = null;
    }
    [SerializeField, Tooltip("プレイヤーのタイプ別のステータスリスト")]
    PlayerTypeInfo[] playerTypeInfoList = null;
    public enum SceneType { Game, StageVoting };
    //後々管理しやすい
    Dictionary<PlayerType, PlayerTypeInfo> dictionaryPlayerTypeInfoByType = new Dictionary<PlayerType, PlayerTypeInfo>();

    static PlayerType[] playerTypes = new PlayerType[PlayerCount.MaxValue];

    public override void MyStart()
    {
        //リストからDictionaryに入れ替える
        foreach (var playerTypeInfo in playerTypeInfoList)
        {
            var playerType = playerTypeInfo.statusObject.Type;
            if (dictionaryPlayerTypeInfoByType.ContainsKey(playerType))
            {
                Debug.LogError("同じタイプのプレイヤーを入れています");
            }
            dictionaryPlayerTypeInfoByType.Add(playerType, playerTypeInfo);
        }
        for (int i = 0; i < (int)PlayerType.None; ++i)
        {
            if (!dictionaryPlayerTypeInfoByType.ContainsKey((PlayerType)i))
            {
                Debug.LogError(((PlayerType)i).ToString() + "のタイプのプレイヤーが入っていません");
            }
        }
        playerTypes[0] = PlayerType.Chick;
        playerTypes[1] = PlayerType.Lion;
        playerTypes[2] = PlayerType.Dog;
        playerTypes[3] = PlayerType.Chick;
    }

    /// <summary>
    /// プレイヤーのタイプをセット
    /// </summary>
    static public void SetPlayerType(int index, PlayerType playerType)
    {
        playerTypes[index] = playerType;
    }

    /// <summary>
    /// プレイヤーを生成して返す
    /// </summary>
    public GameObject GeneratePlayer(int index, SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.Game:
                return Instantiate(dictionaryPlayerTypeInfoByType[playerTypes[index]].gamePlayer);
            case SceneType.StageVoting:
                return Instantiate(dictionaryPlayerTypeInfoByType[playerTypes[index]].stageVotingPlayer);
        }
        return null;
    }

    /// <summary>
    /// プレイヤーのステータスを返す
    /// </summary>
    public PlayerTypeStatusScriptableObject GetPlayerStatus(PlayerType type)
    {
        return dictionaryPlayerTypeInfoByType[type].statusObject;
    }

    /// <summary>
    /// プレイヤーのステータスを返す
    /// </summary>
    public PlayerTypeStatusScriptableObject GetPlayerStatus(int index)
    {
        return GetPlayerStatus(playerTypes[index]);
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
    Rabbit,
    None
}