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
            //ランダムな位置を取得
            var posAndRot = playerController.playerGenerator.GetRandomGenerateTransform();
            //レイを飛ばし、地面に最初から付けた状態で始める
            Ray ray = new Ray(posAndRot.position, -Vector3.up);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, float.MaxValue);
            //位置の更新
            var position = playerController.transform.position;
            position = hit.point;
            position.y += 1;
            playerController.transform.position = position;
            //回転の更新
            playerController.transform.rotation = posAndRot.rotation;
            playerController.rotation = playerController.transform.rotation;
            //カメラの回転も更新
            playerController.cameraController.SetRotation(playerController.transform.eulerAngles);
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