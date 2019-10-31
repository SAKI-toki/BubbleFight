using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ボールの外にいるときのステート
    /// </summary>
    class OutBallState : PlayerStateBase
    {
        public override void OnCollisionEnter(Collision other)
        {
            //タグがBallで、プレイヤーを持っていなかったらそのボールに入る
            if (other.gameObject.tag == "Ball" &&
            other.transform.childCount == 0)
            {
                playerController.transform.parent = other.transform;
                playerController.playerStateManager.TranslationState(new InBallState(other.gameObject));
            }
        }
    }
}