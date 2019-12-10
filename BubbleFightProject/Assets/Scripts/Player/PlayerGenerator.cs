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
        //ランダムな生成位置
        int rand = Random.Range(0, generatePositionAndRotations.Count);
        //ボールの生成
        var ball = Instantiate(ballPrefab);
        ball.transform.SetPositionAndRotation(generatePositionAndRotations[rand].position,
                                                generatePositionAndRotations[rand].rotation);
        var player = PlayerTypeManager.GetInstance().GeneratePlayer(index, PlayerTypeManager.SceneType.Game);
        var playerBehaviour = player.GetComponent<PlayerBehaviour>();
        //番号をセット
        playerBehaviour.SetPlayerNumber(index);
        //プレイヤーにセットする
        playerBehaviour.SetPlayerGenerator(this);
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

/// <summary>
/// 位置と回転を保持するクラス
/// </summary>
public class PositionAndRotation
{
    public Vector3 position;
    public Quaternion rotation;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PositionAndRotation(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}