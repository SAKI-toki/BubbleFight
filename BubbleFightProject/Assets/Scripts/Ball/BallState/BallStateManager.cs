using UnityEngine;

public abstract partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// ボールのステートの基底クラス
    /// </summary>
    protected abstract class BallStateBase
    {
        protected BallBehaviour ballBehaviour;

        /// <summary>
        /// ステートの初期化
        /// </summary>
        public void StateInit(BallBehaviour argBallBehaviour)
        {
            ballBehaviour = argBallBehaviour;
            Init();
        }

        //継承先で定義する
        virtual protected void Init() { }
        virtual public BallStateBase Update() { return this; }
        virtual public void LateUpdate() { }
        virtual public void Destroy() { }
        virtual public void OnCollisionEnter(Collision other) { }
        virtual public void OnCollisionStay(Collision other) { }
        virtual public void OnCollisionExit(Collision other) { }
        virtual public void OnTriggerEnter(Collider other) { }
        virtual public void OnTriggerStay(Collider other) { }
        virtual public void OnTriggerExit(Collider other) { }
    }

    /// <summary>
    /// ボールのステートの管理クラス
    /// </summary>
    protected class BallStateManager
    {

        //現在のステート
        BallStateBase currentBallState;
        //状態を更新する対象のBallBehaviour
        BallBehaviour ballBehaviour;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(BallBehaviour argBallBehaviour, BallStateBase initState)
        {
            ballBehaviour = argBallBehaviour;
            TranslationState(initState);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (IsValidState())
            {
                //ステートの更新
                TranslationState(currentBallState.Update());
            }
        }

        /// <summary>
        /// 物理演算後に実行する
        /// </summary>
        public void LateUpdate()
        {
            if (IsValidState()) currentBallState.LateUpdate();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Destroy()
        {
            if (IsValidState()) currentBallState.Destroy();
        }

        /// <summary>
        /// ステートの遷移
        /// </summary>
        public void TranslationState(BallStateBase nextBallState)
        {
            if (currentBallState == nextBallState) return;

            if (IsValidState()) currentBallState.Destroy();
            currentBallState = nextBallState;
            if (IsValidState()) currentBallState.StateInit(ballBehaviour);
        }

        /// <summary>
        /// ステートが有効かどうか
        /// </summary>
        bool IsValidState()
        {
            return currentBallState != null;
        }

        public void OnCollisionEnter(Collision other) { if (IsValidState()) currentBallState.OnCollisionEnter(other); }
        public void OnCollisionStay(Collision other) { if (IsValidState()) currentBallState.OnCollisionStay(other); }
        public void OnCollisionExit(Collision other) { if (IsValidState()) currentBallState.OnCollisionExit(other); }
        public void OnTriggerEnter(Collider other) { if (IsValidState()) currentBallState.OnTriggerEnter(other); }
        public void OnTriggerStay(Collider other) { if (IsValidState()) currentBallState.OnTriggerStay(other); }
        public void OnTriggerExit(Collider other) { if (IsValidState()) currentBallState.OnTriggerExit(other); }
    }
}
