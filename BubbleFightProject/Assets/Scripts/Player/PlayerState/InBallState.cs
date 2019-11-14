using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("ボールでの移動時の力")]
    float ballMovePower = 10.0f;
    [SerializeField, Tooltip("ボールの回転に加える力の割合"), Range(0, 1)]
    float ballRotationPercentage = 0.8f;

    [SerializeField, Tooltip("ボールに付ける物理マテリアル")]
    PhysicMaterial ballPhysicalMaterial = null;

    [SerializeField, Tooltip("曲がりやすくする重み"), Range(1, 2)]
    float ballEasyCurveWeight = 1.0f;
    [SerializeField, Tooltip("ボールの重さ")]
    float ballMass = 1.0f;

    /// <summary>
    /// ボールの中にいるステート
    /// </summary>
    class InBallState : PlayerStateBase
    {
        BallController ballController = null;

        Vector3 inputDir = Vector3.zero;
        Vector3 lookatDir = Vector3.zero;

        protected override void Init()
        {
            ballController = playerController.transform.parent.GetComponent<BallController>();
            ballController.GetComponent<SphereCollider>().material = playerController.ballPhysicalMaterial;
            ballController.InitializeOnPlayer(playerController.playerNumber, playerController.ballMovePower,
                                                playerController.ballRotationPercentage, playerController.ballMass);
            ballController.SetDestroyEvent(delegate { playerController.transform.parent = null; });
            playerController.transform.localPosition = Vector3.zero;
            PlayerPhysicsSet(false);
        }

        public override PlayerStateBase Update()
        {
            //親オブジェクト(ボール)がなくなったらステート遷移
            if (playerController.transform.parent == null) return new BreakBallState();
            ballController.Move(playerController.ballEasyCurveWeight);
            playerController.PlayerRotation(ballController.GetLookatDir());
            return this;
        }

        public override void Destroy()
        {
            PlayerPhysicsSet(true);
        }

        /// <summary>
        /// プレイヤーの物理演算のオンオフ
        /// </summary>
        void PlayerPhysicsSet(bool enabled)
        {
            //コライダーのオンオフ
            foreach (var collider in playerController.gameObject.GetComponents<Collider>()) collider.enabled = enabled;

            if (enabled)
            {
                //この順番にしないと警告が出る
                playerController.playerRigidbody.isKinematic = false;
                playerController.playerRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            else
            {
                //この順番にしないと警告が出る
                playerController.playerRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                playerController.playerRigidbody.isKinematic = true;
            }
        }
    }
}