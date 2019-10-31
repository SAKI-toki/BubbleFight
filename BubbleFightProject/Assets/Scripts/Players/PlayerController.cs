using UnityEngine;

/// <summary>
/// プレイヤーの制御クラス
/// </summary>
public partial class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの番号"), Range(0, 7)]
    int playerNumber = 0;
    [SerializeField, Tooltip("自分自身のRigidbody")]
    new Rigidbody rigidbody = null;

    PlayerStateManager playerStateManager = new PlayerStateManager();

    void Start()
    {
        playerStateManager.Init(this, new InBallState());
    }

    void Update()
    {
        playerStateManager.Update();
    }

    void OnDestroy()
    {
        playerStateManager.Destroy();
    }
}
