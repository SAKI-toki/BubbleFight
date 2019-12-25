using UnityEngine;

public abstract partial class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// ボールの外にいるときのステート
    /// </summary>
    protected class GameOutBallState : PlayerStateBase
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
            var stickInput = SwitchInput.GetLeftStick(playerBehaviour.playerNumber);

            Vector3 moveDir = new Vector3(stickInput.x, 0, stickInput.y);

            //回転
            playerBehaviour.PlayerRotation(moveDir);
            //位置の更新
            var position = playerBehaviour.transform.position;
            position += moveDir * playerBehaviour.status.WalkSpeed * Time.deltaTime;
            playerBehaviour.transform.position = position;

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
                playerBehaviour.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
            }
            else
            {
                //0.9以下の移動なら歩き
                if (inputDir.magnitude < 0.9f)
                {
                    playerBehaviour.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Walk);
                }
                //0.9以上の移動なら走り
                else
                {
                    playerBehaviour.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                }
            }
        }

        public override void OnCollisionEnter(Collision other)
        {
            //タグがBallで、プレイヤーを持っていなかったらそのボールに入る
            if (other.gameObject.tag == "Ball")
            {
                var ballController = other.gameObject.GetComponent<GameBallController>();
                if (ballController.IsInPlayer())
                {
                    if (!playerBehaviour.IsInvincible())
                    {
                        PointManager.BreakPlayerPointCalculate(ballController, playerBehaviour);
                        playerBehaviour.playerStateManager.TranslationState(new GameHitInPlayerBallState());
                    }
                }
                else
                {
                    playerBehaviour.transform.parent = other.transform;
                    playerBehaviour.playerStateManager.TranslationState(new GameIntoBallState());
                }
            }
        }
    }
}