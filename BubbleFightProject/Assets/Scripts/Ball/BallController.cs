using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MaterialFlash))]
public class BallController : MonoBehaviour
{
    Rigidbody thisRigidbody;
    Vector3 inputDir = Vector3.zero;
    Vector3 addTorque = Vector3.zero;
    Vector3 lookatDir = Vector3.zero;

    //操作するプレイヤー
    int playerIndex = int.MaxValue;
    //移動力
    float movePower = 0.0f;

    [SerializeField, Tooltip("ボールの耐久値(初期値)")]
    float maxHitPoint = 100;

    float hitPoint = 0.0f;

    [SerializeField, Tooltip("与えるダメージ量(指数的に増加)")]
    float damageWeight = 2.0f;
    //当たる前の力を保持する変数
    Vector3 prevVelocity = Vector3.zero;
    //回転に加える力の割合
    float rotationPercentage = 0.0f;
    //入力を受け付けない時間
    float cantInputTime = 0.0f;

    public delegate void DestroyEventType();
    DestroyEventType destroyEvent;

    [SerializeField, Tooltip("最大の回転力")]
    float maxAngularVelocity = 10.0f;
    [SerializeField, Tooltip("入力を受け付けなくする衝突力")]
    float cantInputHitPower = 50.0f;
    [SerializeField, Tooltip("入力を受け付けない最大時間")]
    float maxCantInputTime = 1.0f;
    [SerializeField, Tooltip("力1に対してどのくらい入力を受け付けなくするか(50で0.01なら0.5秒")]
    float hitPowerPercenage = 0.003f;
    //反発時の力の追加
    [SerializeField, Tooltip("ボール同士でぶつかったときの反発の追加率(cantInputHitPower以上の力)")]
    float strongHitBounceAddPower = 1.2f;
    [SerializeField, Tooltip("ボール同士でぶつかったときの反発の追加率(cantInputHitPower以下の力)")]
    float weakHitBounceAddPower = 1.2f;

    MaterialFlash materialFlash;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        materialFlash = GetComponent<MaterialFlash>();
    }

    void Start()
    {
        thisRigidbody.maxAngularVelocity = maxAngularVelocity;
        hitPoint = maxHitPoint;
        materialFlash.SetMaterial(GetComponent<MeshRenderer>().material);
    }

    void Update()
    {
        prevVelocity = thisRigidbody.velocity;
        thisRigidbody.AddForce(Vector3.up * -10);
        //点滅する
        if (IsInPlayer() && hitPoint * 2 < maxHitPoint)
        {
            materialFlash.FlashStart();
            float interval = hitPoint / maxHitPoint;
            materialFlash.SetInterval(interval < 0.1f ? 0.1f : interval);
        }
    }

    /// <summary>
    /// プレイヤーによる初期化
    /// </summary>
    public void InitializeOnPlayer(int index, float ballMovePower,
                                        float ballRotationPowerPercentage, float ballMass)
    {
        playerIndex = index;
        movePower = ballMovePower;
        rotationPercentage = ballRotationPowerPercentage;
        thisRigidbody.mass = ballMass;
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void Move(float easyCurveWeight)
    {
        //入力を受け付けない
        if (cantInputTime > 0.0f)
        {
            cantInputTime -= Time.deltaTime;
            return;
        }

        inputDir.x = SwitchInput.GetHorizontal(playerIndex);
        inputDir.z = SwitchInput.GetVertical(playerIndex);
        //曲がりやすくする
        var velocity = thisRigidbody.velocity;
        velocity.y = 0;
        /*
        力の向きと入力の向きの内積
        同じ向きなら1,反対向きなら-1,垂直なら0のため
        (-dot + 1) / 2をすることで同じ向きなら0,反対向きなら1になるようにする
        */
        float angle = (-Vector3.Dot(velocity.normalized, inputDir.normalized) + 1) / 2;
        addTorque.x = inputDir.z;
        addTorque.z = -inputDir.x;
        float power = movePower * thisRigidbody.mass * Mathf.Pow(angle + 1, easyCurveWeight);
        //入力方向に力を加える
        thisRigidbody.AddForce(inputDir * power * (1.0f - rotationPercentage));
        //入力方向に回転の力を加える
        thisRigidbody.AddTorque(addTorque * power * rotationPercentage);

        if (inputDir.x == 0 && inputDir.z == 0)
        {
            //力のかかっている方向を向く
            lookatDir.x = thisRigidbody.velocity.x;
            lookatDir.z = thisRigidbody.velocity.z;
        }
        else
        {
            //入力方向を向く
            lookatDir.x = inputDir.x;
            lookatDir.z = inputDir.z;
        }
    }

    /// <summary>
    /// 向く方向を返す
    /// </summary>
    public Vector3 GetLookatDir()
    {
        return lookatDir;
    }

    void OnCollisionEnter(Collision other)
    {
        //ボールと衝突
        if (other.gameObject.tag == "Ball")
        {
            var otherBallController = other.gameObject.GetComponent<BallController>();
            //ダメージ(空の場合は10分の1のダメージにする)
            hitPoint -= DamageCalculate(other.relativeVelocity.sqrMagnitude,
                                        otherBallController.prevVelocity.sqrMagnitude, otherBallController.damageWeight) *
                                        (otherBallController.IsInPlayer() ? 1.0f : 0.1f);

            //跳ね返りの強さ
            float bounceAddPower = other.relativeVelocity.sqrMagnitude > cantInputHitPower ?
                                    strongHitBounceAddPower : weakHitBounceAddPower;
            var velocity = thisRigidbody.velocity;
            velocity.x *= bounceAddPower;
            velocity.z *= bounceAddPower;
            thisRigidbody.velocity = velocity;

            //入っていて、力が一定以上なら入力不可時間を与える
            if (otherBallController.IsInPlayer() && other.relativeVelocity.sqrMagnitude > cantInputHitPower)
            {
                cantInputTime = other.relativeVelocity.sqrMagnitude * hitPowerPercenage;
                if (cantInputTime > maxCantInputTime) cantInputTime = maxCantInputTime;
            }

            //HPが0以下になったら破壊
            if (hitPoint <= 0)
            {
                PointManager.BreakBallPointCalculate(otherBallController, this);
                BrokenBall();
            }
        }
        //プレイヤーと衝突
        if (other.gameObject.tag == "Player")
        {
            if (IsInPlayer() && transform.GetChild(0) != other.transform)
            {
                PointManager.BreakPlayerPointCalculate(this, other.gameObject.GetComponent<PlayerController>());
            }
        }
        //マップ外に出た時の処理
        if (other.gameObject.tag == "BreakArea")
        {
            BrokenBall();
        }
    }

    /// <summary>
    /// ボールが破壊された
    /// </summary>
    void BrokenBall()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            transform.GetChild(i).transform.parent = null;
        }
        destroyEvent();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    float DamageCalculate(float collisionPower, float hitObjectPower, float hitBallDamageWeight)
    {
        float damageBase = collisionPower * hitObjectPower /
        (prevVelocity.sqrMagnitude + hitObjectPower);
        return Mathf.Pow(damageBase, hitBallDamageWeight / 10);
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
        destroyEvent = eventType;
    }
}