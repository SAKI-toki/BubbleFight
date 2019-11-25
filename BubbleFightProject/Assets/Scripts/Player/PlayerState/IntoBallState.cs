using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ボールに入る
    /// </summary>
    class IntoBallState : PlayerStateBase
    {
        BallController ballController = null;
        protected override void Init()
        {
            //ボールの初期化
            ballController = playerController.transform.parent.GetComponent<BallController>();
            ballController.StartIntoPlayer(playerController.playerNumber, playerController.playerAnimation,
                                                playerController.cameraController);
            ballController.SetDestroyEvent(delegate { playerController.transform.parent = null; });
        }
        public override PlayerStateBase Update()
        {
            return new InBallState();
        }
        public override void Destroy()
        {
            //入り終わったことを知らせる
            ballController.EndIntoPlayer();
            playerController.invincibleTimeCount = 0.0f;
        }
    }
}