using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
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
            playerController.transform.localPosition = Vector3.zero;
            PlayerPhysicsSet(false);
        }

        public override PlayerStateBase Update()
        {
            //親オブジェクト(ボール)がなくなったらステート遷移
            if (playerController.transform.parent == null) return new BreakBallState();
            ballController.SetPlayerForwardRight(playerController.GetMoveForwardDirection(), playerController.GetMoveRightDirection());
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