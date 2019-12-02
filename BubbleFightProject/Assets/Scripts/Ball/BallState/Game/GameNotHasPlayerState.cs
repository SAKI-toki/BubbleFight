using UnityEngine;

public abstract partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っていないステート
    /// </summary>
    protected class GameNotHasPlayerState : BallStateBase
    {
        public override BallStateBase Update()
        {
            return this;
        }
    }
}
