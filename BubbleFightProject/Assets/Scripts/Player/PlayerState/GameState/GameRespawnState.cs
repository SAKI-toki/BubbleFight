using UnityEngine;

public abstract partial class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// 場外に出てリスポーンするステート
    /// </summary>
    protected class GameRespawnState : PlayerStateBase
    {
        const float animationSwitchTime = 1.0f;
        const float rotationAndScalingTime = 4.0f;
        float timeCount = animationSwitchTime + rotationAndScalingTime;
        Vector3 initScale = Vector3.zero;
        protected override void Init()
        {
            playerBehaviour.invincibleTimeCount = float.MaxValue;
            playerBehaviour.PhysicsSet(false);
            playerBehaviour.canCameraControl = false;
            initScale = playerBehaviour.modelTransform.localScale;
        }

        public override PlayerStateBase Update()
        {
            timeCount -= Time.deltaTime;
            if (timeCount < 0)
            {
                return new GameOutBallState();
            }
            if (timeCount > rotationAndScalingTime)
            {
                playerBehaviour.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Rest);
            }
            else
            {
                playerBehaviour.rotation = playerBehaviour.rotation * Quaternion.Euler(0, 360 * Time.deltaTime, 0);
                playerBehaviour.modelTransform.localScale = Vector3.Lerp(initScale, Vector3.zero, (rotationAndScalingTime - timeCount) / rotationAndScalingTime);
            }
            return this;
        }

        public override void Destroy()
        {
            Respawn();
            playerBehaviour.invincibleTimeCount = PlayerBehaviour.InvincibleTime;
            playerBehaviour.canCameraControl = true;
            playerBehaviour.modelTransform.localScale = initScale;
            playerBehaviour.PhysicsSet(true);
        }

        /// <summary>
        /// リスポーン
        /// </summary>
        void Respawn()
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
        }
    }
}