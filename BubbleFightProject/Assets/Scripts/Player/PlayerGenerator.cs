using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーの生成器
/// </summary>
public class PlayerGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの生成位置")]
    Transform[] generateTransforms = null;

    [SerializeField, Tooltip("ボール")]
    GameObject ballPrefab = null;

    List<PositionAndRotation> generatePositionAndRotations = new List<PositionAndRotation>();

    void Start()
    {
#if UNITY_EDITOR
        var playerTypeManager = PlayerTypeManager.GetInstance();
        playerTypeManager.SetPlayerType(0, PlayerType.Chicken);
        playerTypeManager.SetPlayerType(1, PlayerType.Chicken);
        playerTypeManager.SetPlayerType(2, PlayerType.Chicken);
        playerTypeManager.SetPlayerType(3, PlayerType.Chicken);
#endif // UNITY_EDITOR
        GenerateAllJoinPlayer();
        Physics.Simulate(5.0f);
    }

    /// <summary>
    /// 参加するプレイヤーを生成
    /// </summary>
    void GenerateAllJoinPlayer()
    {
        if (generateTransforms.Length < PlayerJoinManager.GetJoinPlayerCount())
        {
            Debug.LogError("生成位置が参加人数より少ないです");
        }
        generatePositionAndRotations.Clear();
        foreach (var generateTransform in generateTransforms)
        {
            generatePositionAndRotations.Add(new PositionAndRotation(generateTransform.position, generateTransform.rotation));
        }
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (PlayerJoinManager.IsJoin(i))
            {
                GeneratePlayer(i);
            }
        }
    }

    /// <summary>
    /// プレイヤーを生成
    /// </summary>
    void GeneratePlayer(int index)
    {
        var player = PlayerTypeManager.GetInstance().GeneratePlayer(index);
        var playerController = player.GetComponent<PlayerController>();
        //番号をセット
        playerController.SetPlayerNumber(index);
        //ボールの中からのスタートなのでステートを変更
        playerController.SetInitState(PlayerController.PlayerStateEnum.In);
        //ランダムな生成位置
        int rand = Random.Range(0, generatePositionAndRotations.Count);
        //ボールの生成
        var ball = Instantiate(ballPrefab);
        ball.transform.SetPositionAndRotation(generatePositionAndRotations[rand].position,
                                                generatePositionAndRotations[rand].rotation);
        player.transform.parent = ball.transform;
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
        //かぶらないように削除
        generatePositionAndRotations.RemoveAt(rand);
    }

    /// <summary>
    /// ランダムな生成位置を返す
    /// </summary>
    public PositionAndRotation GetRandomGenerateTransform()
    {
        int rand = Random.Range(0, generateTransforms.Length);
        return new PositionAndRotation(generateTransforms[rand].position, generateTransforms[rand].rotation);
    }
}

public class PositionAndRotation
{
    public Vector3 position;
    public Quaternion rotation;
    public PositionAndRotation(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}