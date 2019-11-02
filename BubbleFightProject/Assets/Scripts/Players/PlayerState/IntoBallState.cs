using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ボールに入る
    /// </summary>
    class IntoBallState : PlayerStateBase
    {
        //ボール
        GameObject ball = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IntoBallState(GameObject ballObject)
        {
            ball = ballObject;
        }

        public override PlayerStateBase Update()
        {
            return new InBallState(ball);
        }
    }
}