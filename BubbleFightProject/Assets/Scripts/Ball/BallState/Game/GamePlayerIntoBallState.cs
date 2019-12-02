using UnityEngine;

public abstract partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っている最中のステート
    /// </summary>
    protected class GamePlayerIntoBallState : BallStateBase
    {
        public override BallStateBase Update()
        {
            return this;
        }
    }
}
