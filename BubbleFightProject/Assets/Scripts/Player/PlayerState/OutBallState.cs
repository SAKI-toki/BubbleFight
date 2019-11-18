using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("歩行時の速度")]
    float walkMoveSpeed = 10.0f;

    /// <summary>
    /// ボールの外にいるときのステート
    /// </summary>
    class OutBallState : PlayerStateBase
    {

        public override PlayerStateBase Update()
        {
            Move();
            return this;
        }

        /// <summary>
        /// 移動処理
        /// </summary>
        void Move()
        {
            var stickInput = SwitchInput.GetLeftStick(playerController.playerNumber);

            Vector3 moveDir = PlayerMath.ForwardAndRightMove(stickInput,
                playerController.GetMoveForwardDirection(), playerController.GetMoveRightDirection());
            //回転
            playerController.PlayerRotation(moveDir);
            //位置の更新
            var position = playerController.transform.position;
            position += moveDir * playerController.walkMoveSpeed * Time.deltaTime;
            playerController.transform.position = position;
            if (moveDir.sqrMagnitude != 0)
            {
                playerController.playerAnimation.AnimationSpeed(moveDir.magnitude * 2);
                playerController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Walk);
            }
            else
            {
                playerController.playerAnimation.AnimationSpeed(1.0f);
                playerController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
            }
        }

        public override void OnCollisionEnter(Collision other)
        {
            //タグがBallで、プレイヤーを持っていなかったらそのボールに入る
            if (other.gameObject.tag == "Ball" &&
            other.transform.childCount == 0)
            {
                playerController.transform.parent = other.transform;
                playerController.playerStateManager.TranslationState(new IntoBallState());
            }
        }
    }
}