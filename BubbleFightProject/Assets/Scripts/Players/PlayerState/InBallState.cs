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
        protected override IPlayerState Update()
        {
            BallMove();
            return this;
        }

        /// <summary>
        /// ボールでの移動処理
        /// </summary>
        void BallMove()
        {
            float stickHorizontal = SwitchInput.GetHorizontal(playerController.playerNumber) * playerController.ballMovePower;
            float stickVertical = SwitchInput.GetVertical(playerController.playerNumber) * playerController.ballMovePower;

            playerController.rigidbody.AddForce(stickHorizontal, 0, stickVertical);
        }
    }
}