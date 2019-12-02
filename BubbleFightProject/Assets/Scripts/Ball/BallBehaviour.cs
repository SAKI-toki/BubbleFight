using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MaterialFlash))]
public abstract partial class BallBehaviour : MonoBehaviour
{
    protected MaterialFlash materialFlash;
    protected Rigidbody thisRigidbody;
    protected Vector3 lookatDir = Vector3.zero;

    //操作するプレイヤー
    protected int playerIndex = int.MaxValue;
    //現在のHP
    protected float currentHitPoint = 0.0f;

    //当たる前の力を保持する変数
    protected Vector3 prevVelocity = Vector3.zero;
    protected TpsCamera tpsCamera = null;

    public delegate void DestroyEventType();
    protected DestroyEventType destroyEvent;

    protected BallStateManager ballStateManager = new BallStateManager();

    //プレイヤーのアニメーションを制御するクラス
    protected PlayerAnimationController playerAnimation = null;
    //入力を受け付けない時間
    protected float cantInputTime = 0.0f;
    //ブーストの間隔の時間を測る
    protected float boostIntervalTimeCount = 0.0f;
    //プレイヤーの前方と右のベクトル
    protected Vector3 playerForward = Vector3.zero, playerRight = Vector3.zero;

    [SerializeField, Tooltip("ボールの情報")]
    protected BallScriptableObject ballScriptableObject = null;

    /// <summary>
    /// プレイヤーの前方と右のベクトルをセット
    /// </summary>
    public void SetPlayerForwardRight(Vector3 forward, Vector3 right)
    {
        playerForward = forward;
        playerRight = right;
    }

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        materialFlash = GetComponent<MaterialFlash>();
        BallAwake();
    }

    void Start()
    {
        thisRigidbody.maxAngularVelocity = ballScriptableObject.MaxAngularVelocity;
        currentHitPoint = ballScriptableObject.MaxHitPoint;
        BallStart();
    }

    void Update()
    {
        prevVelocity = thisRigidbody.velocity;
        thisRigidbody.AddForce(Vector3.up * -10);
        BallUpdate();
        ballStateManager.Update();
    }

    void LateUpdate()
    {
        BallLateUpdate();
        RestrictVelocity();
        ballStateManager.LateUpdate();
    }

    void OnDestroy()
    {
        BallOnDestroy();
        ballStateManager.Destroy();
    }

    /// <summary>
    /// 力を制限する
    /// </summary>
    void RestrictVelocity()
    {
        var velocity = thisRigidbody.velocity;
        float magnitude = velocity.magnitude;
        if (magnitude > ballScriptableObject.MaxVelocityMagnitude)
        {
            velocity = velocity / magnitude * ballScriptableObject.MaxVelocityMagnitude;
        }
        thisRigidbody.velocity = velocity;
    }

    /// <summary>
    /// プレイヤーの情報をセット
    /// </summary>
    public void SetPlayerInfo(int index, PlayerAnimationController playerAnimationController, TpsCamera playerTpsCamera)
    {
        playerIndex = index;
        playerAnimation = playerAnimationController;
        tpsCamera = playerTpsCamera;
    }

    /// <summary>
    /// プレイヤーが入り始めた
    /// </summary>
    public void StartIntoPlayer()
    {
        ballStateManager.TranslationState(new GamePlayerIntoBallState());
    }

    /// <summary>
    /// プレイヤーが入り終わった
    /// </summary>
    public void EndIntoPlayer()
    {
        ballStateManager.TranslationState(new GameHasPlayerState());
    }

    /// <summary>
    /// 向く方向の更新
    /// </summary>
    protected void UpdateLookatDirection(Vector3 addPower)
    {
        if (addPower.x == 0 && addPower.z == 0)
        {
            //入力がないときは力のかかっている方向を向く
            lookatDir.x = thisRigidbody.velocity.x;
            lookatDir.z = thisRigidbody.velocity.z;
        }
        else
        {
            //入力方向を向く
            lookatDir.x = addPower.x;
            lookatDir.z = addPower.z;
        }
    }

    void OnCollisionEnter(Collision other) { BallOnCollisionEnter(other); ballStateManager.OnCollisionEnter(other); }
    void OnCollisionStay(Collision other) { BallOnCollisionStay(other); ballStateManager.OnCollisionStay(other); }
    void OnCollisionExit(Collision other) { BallOnCollisionExit(other); ballStateManager.OnCollisionExit(other); }
    void OnTriggerEnter(Collider other) { BallOnTriggerEnter(other); ballStateManager.OnTriggerEnter(other); }
    void OnTriggerStay(Collider other) { BallOnTriggerStay(other); ballStateManager.OnTriggerStay(other); }
    void OnTriggerExit(Collider other) { BallOnTriggerExit(other); ballStateManager.OnTriggerExit(other); }


    /// <summary>
    /// ボールが破壊された
    /// </summary>
    protected void BrokenBall()
    {
        destroyEvent();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 受けるダメージを計算する
    /// </summary>
    protected float DamageCalculate(float collisionPower, float hitObjectPower)
    {
        return DamageCalculator.Damage(collisionPower,
                                        hitObjectPower / (prevVelocity.sqrMagnitude + hitObjectPower));
    }

    /// <summary>
    /// プレイヤーが入っているかどうか
    /// </summary>
    public bool IsInPlayer()
    { return playerIndex != int.MaxValue; }

    /// <summary>
    /// プレイヤーのインデックスを取得
    /// </summary>
    public int GetPlayerIndex()
    { return playerIndex; }


    /// <summary>
    /// 壊れたときのイベントをセット
    /// </summary>
    public void SetDestroyEvent(DestroyEventType eventType)
    {
        destroyEvent += eventType;
    }

    /// <summary>
    /// 向く方向を返す
    /// </summary>
    public Vector3 GetLookatDir()
    {
        return lookatDir;
    }

    /// <summary>
    /// Rigidbodyを取得
    /// </summary>
    public Rigidbody GetRigidbody()
    {
        return thisRigidbody;
    }

    protected virtual void BallAwake() { }
    protected virtual void BallStart() { }
    protected virtual void BallUpdate() { }
    protected virtual void BallLateUpdate() { }
    protected virtual void BallOnDestroy() { }
    protected virtual void BallOnCollisionEnter(Collision other) { }
    protected virtual void BallOnCollisionStay(Collision other) { }
    protected virtual void BallOnCollisionExit(Collision other) { }
    protected virtual void BallOnTriggerEnter(Collider other) { }
    protected virtual void BallOnTriggerStay(Collider other) { }
    protected virtual void BallOnTriggerExit(Collider other) { }
}