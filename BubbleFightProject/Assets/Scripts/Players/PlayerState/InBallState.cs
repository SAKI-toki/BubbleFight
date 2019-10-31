using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("ボールでの移動時の力")]
    float ballMovePower = 10.0f;

    /// <summary>
    /// ボールの中にいるステート
    /// </summary>
    class InBallState : PlayerStateBase
    {
        Rigidbody ballRigidbody = null;
        Transform ballTransform = null;

        Vector3 inputDir = Vector3.zero;
        Vector3 lookatDir = Vector3.zero;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InBallState(GameObject ballObject)
        {
            ballRigidbody = ballObject.GetComponent<Rigidbody>();
            ballTransform = ballObject.transform;
        }

        protected override void Init()
        {
            playerController.transform.localPosition = Vector3.zero;
            PlayerPhysicsSet(false);
        }

        public override PlayerStateBase Update()
        {
            BallMove();
            return this;
        }

        public override void Destroy()
        {
            PlayerPhysicsSet(true);
        }

        /// <summary>
        /// ボールでの移動処理
        /// </summary>
        void BallMove()
        {
            inputDir.x = SwitchInput.GetHorizontal(playerController.playerNumber);
            inputDir.z = SwitchInput.GetVertical(playerController.playerNumber);
            //入力方向に力を加える
            ballRigidbody.AddForce(inputDir * playerController.ballMovePower);

            if (inputDir.x == 0 && inputDir.z == 0)
            {
                //力のかかっている方向を向く
                lookatDir.x = ballRigidbody.velocity.x;
                lookatDir.z = ballRigidbody.velocity.z;
            }
            else
            {
                //入力方向を向く
                lookatDir.x = inputDir.x;
                lookatDir.z = inputDir.z;
            }

            //プレイヤーの回転
            var startQ = playerController.transform.rotation;
            playerController.transform.LookAt(playerController.transform.position + lookatDir);
            var endQ = playerController.transform.rotation;
            playerController.transform.rotation = Quaternion.Lerp(startQ, endQ, 0.3f);
        }

        /// <summary>
        /// プレイヤーの物理演算のオンオフ
        /// </summary>
        void PlayerPhysicsSet(bool enabled)
        {
            //コライダーのオンオフ
            foreach (var collider in playerController.gameObject.GetComponents<Collider>()) collider.enabled = enabled;

            //Rigidbodyのオンオフ
            var rigidbody = playerController.GetComponent<Rigidbody>();
            if (enabled) rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            else rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.isKinematic = !enabled;

        }
    }
}