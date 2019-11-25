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
            playerController.PhysicsSet(false);
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
            playerController.PhysicsSet(true);
            playerController.invincibleTimeCount = float.MaxValue;

            var localPosition = playerController.modelTransform.localPosition;
            localPosition.y = -1;
            playerController.modelTransform.localPosition = localPosition;
        }
    }
}