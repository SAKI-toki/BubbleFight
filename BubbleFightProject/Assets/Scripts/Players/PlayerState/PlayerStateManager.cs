using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのステートのインターフェース
    /// </summary>
    interface IPlayerState
    {
        void StateInit(PlayerController playerController);
        IPlayerState StateUpdate();
        void StateDestroy();
    }

    /// <summary>
    /// プレイヤーのステートの基底クラス
    /// </summary>
    abstract class PlayerStateBase : IPlayerState
    {
        protected PlayerController playerController;

        void IPlayerState.StateInit(PlayerController argPlayerController)
        {
            playerController = argPlayerController;
            Init();
        }

        IPlayerState IPlayerState.StateUpdate()
        {
            return Update();
        }

        void IPlayerState.StateDestroy()
        {
            Destroy();
        }

        //継承先で定義する
        virtual protected void Init() { }
        virtual protected IPlayerState Update() { return this; }
        virtual protected void Destroy() { }
    }

    /// <summary>
    /// プレイヤーのステートを管理するクラス
    /// </summary>
    class PlayerStateManager
    {
        //現在のステート
        IPlayerState currentPlayerState;
        //状態を更新する対象のPlayerController
        PlayerController playerController;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(PlayerController argPlayerController, IPlayerState initState)
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
                TranslationState(currentPlayerState.StateUpdate());
            }
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Destroy()
        {
            if (IsValidState()) currentPlayerState.StateDestroy();
        }

        /// <summary>
        /// ステートの遷移
        /// </summary>
        public void TranslationState(IPlayerState nextPlayerState)
        {
            if (IsValidState()) currentPlayerState.StateDestroy();
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
    }
}