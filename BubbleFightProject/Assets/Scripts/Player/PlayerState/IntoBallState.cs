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
    }
}