using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ボールが壊れる
    /// </summary>
    class BreakBallState : PlayerStateBase
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
            playerController.invincibleTimeCount = 3.0f;
        }
    }
}