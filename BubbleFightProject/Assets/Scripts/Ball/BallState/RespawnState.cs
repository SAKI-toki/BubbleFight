using UnityEngine;

public partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// リスポーンステート
    /// </summary>
    protected class RespawnState : BallStateBase
    {
        protected override void Init()
        {
            ballBehaviour.thisRigidbody.velocity = Vector3.zero;
            SwitchPhysics(false);
        }

        public override BallStateBase Update()
        {
            return new HasPlayerState();
        }

        public override void Destroy()
        {
            TranslateInitPosition();
            SwitchPhysics(true);
        }

        /// <summary>
        /// 物理演算のオンオフ
        /// </summary>
        void SwitchPhysics(bool isOn)
        {
            //この順番にしないとエラーが出る
            if (isOn)
            {
                foreach (var collider in ballBehaviour.GetComponents<Collider>()) collider.enabled = isOn;
                ballBehaviour.thisRigidbody.isKinematic = !isOn;
                ballBehaviour.thisRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            else
            {
                foreach (var collider in ballBehaviour.GetComponents<Collider>()) collider.enabled = isOn;
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
            pos.y += 10;
            ballBehaviour.transform.position = pos;
            ballBehaviour.transform.rotation = ballBehaviour.initRotation;
        }
    }
}
