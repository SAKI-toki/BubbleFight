using UnityEngine;

public abstract partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// ボールが壊れるステート
    /// </summary>
    protected class GameBreakBallState : BallStateBase
    {
        public override BallStateBase Update()
        {
            return this;
        }
    }
}
