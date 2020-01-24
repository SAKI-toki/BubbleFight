using UnityEngine;

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

        var uiPlayer = PlayerTypeManager.GetInstance().GeneratePlayer(index, PlayerTypeManager.SceneType.Object);
        ballBehaviour.SetUiPlayerAnim(uiPlayer.GetComponent<PlayerAnimationController>());
        uiPlayer.layer = LayerMask.NameToLayer("3DUI");
        foreach (var uiPlayerTransform in uiPlayer.GetComponentsInChildren<Transform>())
        {
            uiPlayerTransform.gameObject.layer = LayerMask.NameToLayer("3DUI");
        }
        const float length = 2.0f;
        switch (index)
        {
            case 0:
                uiPlayer.transform.position = new Vector3(0, 0, length);
                break;
            case 1:
                uiPlayer.transform.position = new Vector3(length, 0, 0);
                break;
            case 2:
                uiPlayer.transform.position = new Vector3(0, 0, -length);
                break;
            case 3:
                uiPlayer.transform.position = new Vector3(-length, 0, 0);
                break;
        }
        uiPlayer.transform.LookAt(Vector3.zero);
        var uiPos = uiPlayer.transform.position;
        uiPos.y = PlayerTypeManager.GetInstance().GetUiOffset(index);
        uiPlayer.transform.position = uiPos;
    }
}
