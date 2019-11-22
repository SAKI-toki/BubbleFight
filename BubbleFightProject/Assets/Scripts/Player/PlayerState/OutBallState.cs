using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
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
            position += moveDir * playerController.status.WalkSpeed * Time.deltaTime;
            playerController.transform.position = position;

            UpdateAnimation(stickInput);
        }

        /// <summary>
        /// アニメーションの更新
        /// </summary>
        void UpdateAnimation(Vector2 inputDir)
        {
            //移動しないならIdle
            if (inputDir.sqrMagnitude == 0)
            {
                playerController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
            }
            else
            {
                //0.9以下の移動なら歩き
                if (inputDir.magnitude < 0.9f)
                {
                    playerController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Walk);
                }
                //0.9以上の移動なら走り
                else
                {
                    playerController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                }
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