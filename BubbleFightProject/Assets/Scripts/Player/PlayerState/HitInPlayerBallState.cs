using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っているボールにぶつかった
    /// </summary>
    class HitInPlayerBallState : PlayerStateBase
    {
        protected override void Init()
        {
            playerController.invincibleTimeCount = float.MaxValue;
        }

        public override PlayerStateBase Update()
        {
            return new OutBallState();
        }

        public override void Destroy()
        {
            playerController.invincibleTimeCount = PlayerController.InvincibleTime;
        }
    }
}