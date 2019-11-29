using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MaterialFlash))]
public partial class BallController : MonoBehaviour
{
    MaterialFlash materialFlash;
    Rigidbody thisRigidbody;
    Vector3 lookatDir = Vector3.zero;

    //操作するプレイヤー
    int playerIndex = int.MaxValue;

    [SerializeField, Tooltip("ボールの耐久値(初期値)")]
    float maxHitPoint = 100;
    //現在のHP
    float currentHitPoint = 0.0f;

    //当たる前の力を保持する変数
    Vector3 prevVelocity = Vector3.zero;
    TpsCamera tpsCamera = null;

    public delegate void DestroyEventType();
    DestroyEventType destroyEvent;

    [SerializeField, Tooltip("最大の回転力")]
    float maxAngularVelocity = 10.0f;
    //反発時の力の追加
    [SerializeField, Tooltip("ボール同士でぶつかったときの反発の追加率(cantInputHitPower以上の力)")]
    float strongHitBounceAddPower = 1.2f;
    [SerializeField, Tooltip("ボール同士でぶつかったときの反発の追加率(cantInputHitPower以下の力)")]
    float weakHitBounceAddPower = 1.2f;
    [SerializeField, Tooltip("最大の力")]
    float maxVelocityMagnitude = 100.0f;

    BallStateManager ballStateManager = new BallStateManager();

    bool isGame = true;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        materialFlash = GetComponent<MaterialFlash>();
    }

    void Start()
    {
        thisRigidbody.maxAngularVelocity = maxAngularVelocity;
        currentHitPoint = maxHitPoint;
        ballStateManager.Init(this, new NotHasPlayerState());
    }

    void Update()
    {
        prevVelocity = thisRigidbody.velocity;
        thisRigidbody.AddForce(Vector3.up * -10);
        ballStateManager.Update();
    }

    void LateUpdate()
    {
        RestrictVelocity();
        ballStateManager.LateUpdate();
    }

    void OnDestroy()
    {
        ballStateManager.Destroy();
    }

    /// <summary>
    /// 力を制限する
    /// </summary>
    void RestrictVelocity()
    {
        var velocity = thisRigidbody.velocity;
        float magnitude = velocity.magnitude;
        if (magnitude > maxVelocityMagnitude)
        {
            velocity = velocity / magnitude * maxVelocityMagnitude;
        }
        thisRigidbody.velocity = velocity;
    }

    /// <summary>
    /// プレイヤーが入り始めたと同時に必要な情報を与える
    /// </summary>
    public void StartIntoPlayer(int index, PlayerAnimationController playerAnimationController, TpsCamera playerTpsCamera)
    {
        playerIndex = index;
        playerAnimation = playerAnimationController;
        tpsCamera = playerTpsCamera;
        ballStateManager.TranslationState(new PlayerIntoBallState());
    }

    /// <summary>
    /// プレイヤーが入り終わった
    /// </summary>
    public void EndIntoPlayer()
    {
        ballStateManager.TranslationState(new HasPlayerState());
    }

    /// <summary>
    /// 向く方向の更新
    /// </summary>
    void UpdateLookatDirection(Vector3 addPower)
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


    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Ball":
                CollisionBall(other);
                break;
            case "BreakArea":
                //マップ外に出た時の処理
                {
                    BrokenBall();
                }
                break;
        }
        ballStateManager.OnCollisionEnter(other);
    }
    void OnCollisionStay(Collision other) { ballStateManager.OnCollisionStay(other); }
    void OnCollisionExit(Collision other) { ballStateManager.OnCollisionExit(other); }
    void OnTriggerEnter(Collider other) { ballStateManager.OnTriggerEnter(other); }
    void OnTriggerStay(Collider other) { ballStateManager.OnTriggerStay(other); }
    void OnTriggerExit(Collider other) { ballStateManager.OnTriggerExit(other); }

    /// <summary>
    /// ボールとの衝突
    /// </summary>
    void CollisionBall(Collision other)
    {
        var otherBallController = other.gameObject.GetComponent<BallController>();
        //ダメージ(空の場合は10分の1のダメージにする)
        currentHitPoint -= DamageCalculate(other.relativeVelocity.sqrMagnitude, otherBallController.prevVelocity.sqrMagnitude)
                            * (otherBallController.IsInPlayer() ? 1.0f : 0.1f);

        //跳ね返りの強さ
        float bounceAddPower = other.relativeVelocity.sqrMagnitude > cantInputHitPower ?
                                strongHitBounceAddPower : weakHitBounceAddPower;
        var velocity = thisRigidbody.velocity;
        velocity.x *= bounceAddPower;
        velocity.z *= bounceAddPower;
        thisRigidbody.velocity = velocity;

        //最後にぶつかったプレイヤーの更新
        LastHitPlayerManager.SetLastHitPlayer(GetPlayerIndex(), otherBallController.GetPlayerIndex());

        //HPが0以下になったら破壊
        if (currentHitPoint <= 0)
        {
            PointManager.BreakBallPointCalculate(otherBallController, this);
            BrokenBall();
        }
    }

    /// <summary>
    /// ボールが破壊された
    /// </summary>
    void BrokenBall()
    {
        destroyEvent();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 受けるダメージを計算する
    /// </summary>
    float DamageCalculate(float collisionPower, float hitObjectPower)
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

    /// <summary>
    /// ゲームかどうかセットする
    /// </summary>
    public void SetIsGame(bool isGameValue)
    {
        isGame = isGameValue;
    }
}