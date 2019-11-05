using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("ボールでの移動時の力")]
    float ballMovePower = 10.0f;

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
            ballController.Initialize(playerController.playerNumber, playerController.ballMovePower);
            playerController.transform.localPosition = Vector3.zero;
            PlayerPhysicsSet(false);
        }

        public override PlayerStateBase Update()
        {
            ballController.Move();
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

            //Rigidbodyのオンオフ
            var rigidbody = playerController.GetComponent<Rigidbody>();
            if (enabled)
            {
                //この順番にしないと警告が出る
                rigidbody.isKinematic = false;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            else
            {
                //この順番にしないと警告が出る
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rigidbody.isKinematic = true;
            }

        }
    }
}