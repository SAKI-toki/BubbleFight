using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ボールが壊れる
    /// </summary>
    class BreakBallState : PlayerStateBase
    {
        public override PlayerStateBase Update()
        {
            return new OutBallState();
        }
    }
}