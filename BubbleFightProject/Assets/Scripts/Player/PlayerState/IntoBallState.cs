using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ボールに入る
    /// </summary>
    class IntoBallState : PlayerStateBase
    {
        public override PlayerStateBase Update()
        {
            return new InBallState();
        }
        public override void Destroy()
        {
            playerController.invincibleTimeCount = 0.0f;
        }
    }
}