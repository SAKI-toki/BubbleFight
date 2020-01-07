using UnityEngine;

/// <summary>
/// ボールの動作
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public abstract partial class BallBehaviour : MonoBehaviour
{
    protected Rigidbody thisRigidbody;
    protected Vector3 lookatDir = Vector3.zero;

    //操作するプレイヤー
    protected int playerIndex = int.MaxValue;

    //当たる前の力を保持する変数
    protected Vector3 prevVelocity = Vector3.zero;

    protected BallStateManager ballStateManager = new BallStateManager();

    //入力を受け付けない時間
    protected float cantInputTime = 0.0f;
    //ブーストの間隔の時間を測る
    protected float boostIntervalTimeCount = 0.0f;

    [SerializeField, Tooltip("ボールの情報")]
    protected BallScriptableObject ballScriptableObject = null;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        BallAwake();
    }

    void Start()
    {
        thisRigidbody.maxAngularVelocity = ballScriptableObject.MaxAngularVelocity;
        BallStart();
    }

    void Update()
    {
        prevVelocity = thisRigidbody.velocity;
        thisRigidbody.AddForce(Vector3.up * -ballScriptableObject.Gravity);
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
        velocity.y = Mathf.Clamp(velocity.y, float.MinValue, 0.0f);
        thisRigidbody.velocity = velocity;
    }

    /// <summary>
    /// 入力不可時間をセット
    /// /// </summary>
    void SetCantInputTime(float time)
    {
        cantInputTime = time;
        if (cantInputTime > ballScriptableObject.MaxCantInputTime)
        {
            cantInputTime = ballScriptableObject.MaxCantInputTime;
        }
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
    /// プレイヤーのインデックスをセット
    /// </summary>
    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
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