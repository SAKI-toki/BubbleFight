using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("歩行時の速度")]
    float walkMoveSpeed = 10.0f;

    /// <summary>
    /// ボールの外にいるときのステート
    /// </summary>
    class OutBallState : PlayerStateBase
    {

        public override PlayerStateBase Update()
        {
            Move();
            return this;
        }

        /// <summary>
        /// 移動処理
        /// </summary>
        void Move()
        {
            var stickInput = SwitchInput.GetLeftStick(playerController.playerNumber);

            Vector3 inputDir = Vector3.zero;
            inputDir.x = stickInput.x;
            inputDir.z = stickInput.y;
            //回転
            playerController.PlayerRotation(inputDir);
            //位置の更新
            var position = playerController.transform.position;
            position += inputDir * playerController.walkMoveSpeed * Time.deltaTime;
            playerController.transform.position = position;
        }

        public override void OnCollisionEnter(Collision other)
        {
            //タグがBallで、プレイヤーを持っていなかったらそのボールに入る
            if (other.gameObject.tag == "Ball" &&
            other.transform.childCount == 0)
            {
                playerController.transform.parent = other.transform;
                playerController.playerStateManager.TranslationState(new IntoBallState());
            }
        }
    }
}