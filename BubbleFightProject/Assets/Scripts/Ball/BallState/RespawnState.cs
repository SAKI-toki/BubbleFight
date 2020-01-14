using UnityEngine;

public partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// リスポーンステート
    /// </summary>
    protected class RespawnState : BallStateBase
    {
        float respawnTimeCount = 0.0f;
        const float RespawnDelay = 3.0f;
        protected override void Init()
        {
            ballBehaviour.thisRigidbody.velocity = Vector3.zero;
            var pos = ballBehaviour.initPosition;
            pos.y += 10;
            ballBehaviour.transform.position = pos;
            ballBehaviour.transform.rotation = ballBehaviour.initRotation;
            ballBehaviour.boostIntervalTimeCount = 0.0f;
        }

        public override BallStateBase Update()
        {
            return new HasPlayerState();
        }
    }
}
