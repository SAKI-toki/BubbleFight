using UnityEngine;

public abstract partial class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// ステージ選択時のボールに入っているステート
    /// </summary>
    protected class StageVotingInBallState : PlayerStateBase
    {
        StageVotingBallController ballController = null;
        protected override void Init()
        {
            //ボールの初期化
            ballController = playerBehaviour.transform.parent.GetComponent<StageVotingBallController>();
            ballController.SetPlayerInfo(playerBehaviour.playerNumber, playerBehaviour.playerAnimation);
            playerBehaviour.transform.localPosition = Vector3.zero;
            playerBehaviour.PhysicsSet(false);
        }

        public override PlayerStateBase Update()
        {
            playerBehaviour.PlayerRotation(ballController.GetLookatDir());
            return this;
        }

        public override void Destroy()
        {
        }
    }
}