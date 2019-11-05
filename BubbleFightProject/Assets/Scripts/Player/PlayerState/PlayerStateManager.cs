using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのステートの基底クラス
    /// </summary>
    abstract class PlayerStateBase
    {
        protected PlayerController playerController;

        /// <summary>
        /// ステートの初期化
        /// </summary>
        public void StateInit(PlayerController argPlayerController)
        {
            playerController = argPlayerController;
            Init();
        }

        //継承先で定義する
        virtual protected void Init() { }
        virtual public PlayerStateBase Update() { return this; }
        virtual public void Destroy() { }
        virtual public void OnCollisionEnter(Collision other) { }
        virtual public void OnCollisionStay(Collision other) { }
        virtual public void OnCollisionExit(Collision other) { }
        virtual public void OnTriggerEnter(Collider other) { }
        virtual public void OnTriggerStay(Collider other) { }
        virtual public void OnTriggerExit(Collider other) { }
    }

    /// <summary>
    /// プレイヤーのステートを管理するクラス
    /// </summary>
    class PlayerStateManager
    {
        //現在のステート
        PlayerStateBase currentPlayerState;
        //状態を更新する対象のPlayerController
        PlayerController playerController;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(PlayerController argPlayerController, PlayerStateBase initState)
        {
            playerController = argPlayerController;
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
                TranslationState(currentPlayerState.Update());
            }
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Destroy()
        {
            if (IsValidState()) currentPlayerState.Destroy();
        }

        /// <summary>
        /// ステートの遷移
        /// </summary>
        public void TranslationState(PlayerStateBase nextPlayerState)
        {
            if (currentPlayerState == nextPlayerState) return;

            if (IsValidState()) currentPlayerState.Destroy();
            currentPlayerState = nextPlayerState;
            if (IsValidState()) currentPlayerState.StateInit(playerController);
        }

        /// <summary>
        /// ステートが有効かどうか
        /// </summary>
        bool IsValidState()
        {
            return currentPlayerState != null;
        }

        public void OnCollisionEnter(Collision other) { if (IsValidState()) currentPlayerState.OnCollisionEnter(other); }
        public void OnCollisionStay(Collision other) { if (IsValidState()) currentPlayerState.OnCollisionStay(other); }
        public void OnCollisionExit(Collision other) { if (IsValidState()) currentPlayerState.OnCollisionExit(other); }
        public void OnTriggerEnter(Collider other) { if (IsValidState()) currentPlayerState.OnTriggerEnter(other); }
        public void OnTriggerStay(Collider other) { if (IsValidState()) currentPlayerState.OnTriggerStay(other); }
        public void OnTriggerExit(Collider other) { if (IsValidState()) currentPlayerState.OnTriggerExit(other); }
    }
}