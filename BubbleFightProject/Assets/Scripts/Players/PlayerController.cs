using UnityEngine;

/// <summary>
/// プレイヤーの制御クラス
/// </summary>
public partial class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの番号"), Range(0, 7)]
    int playerNumber = 0;

    PlayerStateManager playerStateManager = new PlayerStateManager();

    void Start()
    {
        playerStateManager.Init(this, new OutBallState());
    }

    void Update()
    {
        playerStateManager.Update();
    }

    void OnDestroy()
    {
        playerStateManager.Destroy();
    }

    void OnCollisionEnter(Collision other) { playerStateManager.OnCollisionEnter(other); }
    void OnCollisionStay(Collision other) { playerStateManager.OnCollisionStay(other); }
    void OnCollisionExit(Collision other) { playerStateManager.OnCollisionExit(other); }
    void OnTriggerEnter(Collider other) { playerStateManager.OnTriggerEnter(other); }
    void OnTriggerStay(Collider other) { playerStateManager.OnTriggerStay(other); }
    void OnTriggerExit(Collider other) { playerStateManager.OnTriggerExit(other); }
}
