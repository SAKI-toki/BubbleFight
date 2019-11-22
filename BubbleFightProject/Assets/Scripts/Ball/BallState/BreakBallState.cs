using UnityEngine;

public partial class BallController : MonoBehaviour
{
    /// <summary>
    /// ボールが壊れるステート
    /// </summary>
    class BreakBallState : BallStateBase
    {
        public override BallStateBase Update()
        {
            return this;
        }
    }
}
