using UnityEngine;

public partial class BallController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っている最中のステート
    /// </summary>
    class PlayerIntoBallState : BallStateBase
    {
        public override BallStateBase Update()
        {
            return this;
        }
    }
}
