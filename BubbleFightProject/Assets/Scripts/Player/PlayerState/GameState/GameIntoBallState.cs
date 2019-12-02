using UnityEngine;

public abstract partial class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// ボールに入る
    /// </summary>
    protected class GameIntoBallState : PlayerStateBase
    {
        GameBallController ballController = null;
        protected override void Init()
        {
            //ボールの初期化
            ballController = playerBehaviour.transform.parent.GetComponent<GameBallController>();
            ballController.SetPlayerInfo(playerBehaviour.playerNumber, playerBehaviour.playerAnimation,
                                                playerBehaviour.cameraController);
            ballController.StartIntoPlayer();
            ballController.SetDestroyEvent(delegate { playerBehaviour.transform.parent = null; });
        }
        public override PlayerStateBase Update()
        {
            var localPosition = playerBehaviour.modelTransform.localPosition;
            localPosition.y = playerBehaviour.inBallModelLocalPositionY;
            playerBehaviour.modelTransform.localPosition = localPosition;
            return new GameInBallState();
        }
        public override void Destroy()
        {
            //入り終わったことを知らせる
            ballController.EndIntoPlayer();
            playerBehaviour.invincibleTimeCount = 0.0f;
        }
    }
}