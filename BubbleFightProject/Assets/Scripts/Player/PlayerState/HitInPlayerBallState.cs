using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っているボールにぶつかった
    /// </summary>
    class HitInPlayerBallState : PlayerStateBase
    {
        float time = 3.0f;

        protected override void Init()
        {
            playerController.invincibleTimeCount = float.MaxValue;
            playerController.PhysicsSet(false);
            playerController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Rest);
        }

        public override PlayerStateBase Update()
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                return new OutBallState();
            }
            return this;
        }

        public override void Destroy()
        {
            playerController.invincibleTimeCount = PlayerController.InvincibleTime;
            playerController.PhysicsSet(true);
        }
    }
}