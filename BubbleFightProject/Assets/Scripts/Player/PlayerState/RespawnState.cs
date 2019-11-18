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
            Ray ray = new Ray(Vector3.zero, -Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                var position = playerController.transform.position;
                position = hit.point;
                position.y += 1;
                playerController.transform.position = position;
            }
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