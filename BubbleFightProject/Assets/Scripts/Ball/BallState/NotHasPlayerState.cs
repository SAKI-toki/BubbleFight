using UnityEngine;

public partial class BallController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っていないステート
    /// </summary>
    class NotHasPlayerState : BallStateBase
    {
        public override BallStateBase Update()
        {
            return this;
        }
    }
}
