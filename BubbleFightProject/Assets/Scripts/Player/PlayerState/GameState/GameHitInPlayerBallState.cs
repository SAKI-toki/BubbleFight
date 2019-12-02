using UnityEngine;

public abstract partial class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っているボールにぶつかった
    /// </summary>
    protected class GameHitInPlayerBallState : PlayerStateBase
    {
        float time = 3.0f;

        protected override void Init()
        {
            playerBehaviour.invincibleTimeCount = float.MaxValue;
            playerBehaviour.PhysicsSet(false);
            playerBehaviour.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Rest);
        }

        public override PlayerStateBase Update()
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                return new GameOutBallState();
            }
            return this;
        }

        public override void Destroy()
        {
            playerBehaviour.invincibleTimeCount = PlayerBehaviour.InvincibleTime;
            playerBehaviour.PhysicsSet(true);
        }
    }
}