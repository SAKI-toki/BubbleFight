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
        //ボールの生成
        var ball = Instantiate(ballPrefab);
        ball.transform.SetPositionAndRotation(generateTransforms[index].position,
                                                generateTransforms[index].rotation);
        var player = PlayerTypeManager.GetInstance().GeneratePlayer(index, PlayerTypeManager.SceneType.Game);

        var ballBehaviour = ball.GetComponent<BallBehaviour>();
        ballBehaviour.SetPlayerIndex(index);
        //プレイヤーにセットする
        player.transform.parent = ball.transform;
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
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