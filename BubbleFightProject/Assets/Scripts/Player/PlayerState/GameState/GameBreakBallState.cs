using UnityEngine;

public abstract partial class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// ボールが壊れる
    /// </summary>
    protected class GameBreakBallState : PlayerStateBase
    {
        protected override void Init()
        {
            playerBehaviour.invincibleTimeCount = float.MaxValue;
        }

        public override PlayerStateBase Update()
        {
            return new GameOutBallState();
        }

        public override void Destroy()
        {
            playerBehaviour.invincibleTimeCount = 3.0f;
        }
    }
}