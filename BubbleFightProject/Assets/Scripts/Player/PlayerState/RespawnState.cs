using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 場外に出てリスポーンするステート
    /// </summary>
    class RespawnState : PlayerStateBase
    {
        protected override void Init()
        {
            playerController.transform.position = Vector3.zero;
            playerController.invincibleTimeCount = float.MaxValue;
        }

        public override PlayerStateBase Update()
        {
            return new OutBallState();
        }

        public override void Destroy()
        {
            playerController.invincibleTimeCount = PlayerController.InvincibleTime;
        }
    }
}