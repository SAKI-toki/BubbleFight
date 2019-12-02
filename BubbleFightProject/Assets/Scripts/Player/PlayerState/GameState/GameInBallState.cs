using UnityEngine;

public abstract partial class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// ボールの中にいるステート
    /// </summary>
    protected class GameInBallState : PlayerStateBase
    {
        GameBallController ballController = null;

        Vector3 inputDir = Vector3.zero;
        Vector3 lookatDir = Vector3.zero;

        protected override void Init()
        {
            ballController = playerBehaviour.transform.parent.GetComponent<GameBallController>();
            playerBehaviour.transform.localPosition = Vector3.zero;
            playerBehaviour.PhysicsSet(false);
        }

        public override PlayerStateBase Update()
        {
            //親オブジェクト(ボール)がなくなったらステート遷移
            if (playerBehaviour.transform.parent == null) return new GameBreakBallState();
            ballController.SetPlayerForwardRight(playerBehaviour.GetMoveForwardDirection(), playerBehaviour.GetMoveRightDirection());
            playerBehaviour.PlayerRotation(ballController.GetLookatDir());
            return this;
        }

        public override void Destroy()
        {
            playerBehaviour.PhysicsSet(true);
            playerBehaviour.invincibleTimeCount = float.MaxValue;

            var localPosition = playerBehaviour.modelTransform.localPosition;
            localPosition.y = -1;
            playerBehaviour.modelTransform.localPosition = localPosition;
        }
    }
}