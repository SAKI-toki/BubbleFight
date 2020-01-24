using UnityEngine;

public partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// リスポーンステート
    /// </summary>
    protected class RespawnState : BallStateBase
    {
        float timeCount = 0.0f;
        float respawnDelayTime = 0.0f;
        const float MaxRespawnDelayTime = 5.0f;

        protected override void Init()
        {
            ballBehaviour.thisRigidbody.velocity = Vector3.zero;
            SwitchEnable(false);
            ++ballBehaviour.destroyCount;
            respawnDelayTime = Mathf.Clamp(ballBehaviour.destroyCount * 1.0f, 0.0f, MaxRespawnDelayTime);
            if (ballBehaviour.numberUiTransform) ballBehaviour.numberUiTransform.gameObject.SetActive(false);
            if (ballBehaviour.uiPlayerAnim) ballBehaviour.uiPlayerAnim.AnimationSwitch(PlayerAnimationController.AnimationType.Rest);
        }

        public override BallStateBase Update()
        {
            timeCount += Time.deltaTime;
            if (timeCount > respawnDelayTime)
            {
                return new HasPlayerState();
            }
            return this;
        }

        public override void Destroy()
        {
            TranslateInitPosition();
            SwitchEnable(true);
            if (ballBehaviour.numberUiTransform) ballBehaviour.numberUiTransform.gameObject.SetActive(true);
            if (ballBehaviour.uiPlayerAnim) ballBehaviour.uiPlayerAnim.AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
        }

        /// <summary>
        /// オンオフ
        /// </summary>
        void SwitchEnable(bool isOn)
        {
            foreach (var collider in ballBehaviour.GetComponents<Collider>()) collider.enabled = isOn;
            foreach (var collider in ballBehaviour.GetComponentsInChildren<Collider>()) collider.enabled = isOn;
            foreach (var renderer in ballBehaviour.GetComponents<Renderer>()) renderer.enabled = isOn;
            foreach (var renderer in ballBehaviour.GetComponentsInChildren<Renderer>()) renderer.enabled = isOn;
            //この順番にしないとエラーが出る
            if (isOn)
            {
                ballBehaviour.thisRigidbody.isKinematic = !isOn;
                ballBehaviour.thisRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            else
            {
                ballBehaviour.thisRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                ballBehaviour.thisRigidbody.isKinematic = !isOn;
            }
        }

        /// <summary>
        /// 初期位置に移動
        /// </summary>
        void TranslateInitPosition()
        {
            var pos = ballBehaviour.initPosition;
            pos.y += 3;
            ballBehaviour.transform.position = pos;
            ballBehaviour.transform.rotation = ballBehaviour.initRotation;
        }
    }
}
