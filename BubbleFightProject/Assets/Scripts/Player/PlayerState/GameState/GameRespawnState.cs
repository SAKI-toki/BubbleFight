using UnityEngine;

public abstract partial class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// 場外に出てリスポーンするステート
    /// </summary>
    protected class GameRespawnState : PlayerStateBase
    {
        protected override void Init()
        {
            //ランダムな位置を取得
            var posAndRot = playerBehaviour.playerGenerator.GetRandomGenerateTransform();
            //レイを飛ばし、地面に最初から付けた状態で始める
            Ray ray = new Ray(posAndRot.position, -Vector3.up);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, float.MaxValue);
            //位置の更新
            var position = playerBehaviour.transform.position;
            position = hit.point;
            position.y += 1;
            playerBehaviour.transform.position = position;
            //回転の更新
            playerBehaviour.transform.rotation = posAndRot.rotation;
            playerBehaviour.rotation = playerBehaviour.transform.rotation;
            //カメラの回転も更新
            playerBehaviour.cameraController.SetRotation(playerBehaviour.transform.eulerAngles);
            playerBehaviour.invincibleTimeCount = float.MaxValue;
        }

        public override PlayerStateBase Update()
        {
            return new GameOutBallState();
        }

        public override void Destroy()
        {
            playerBehaviour.invincibleTimeCount = PlayerBehaviour.InvincibleTime;
        }
    }
}