using UnityEngine;

/// <summary>
/// プレイヤーの制御クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MaterialFlash))]
public partial class PlayerController : MonoBehaviour
{
    Rigidbody playerRigidbody = null;
    MaterialFlash materialFlash = null;
    [SerializeField, Tooltip("プレイヤーの番号"), Range(0, 7)]
    int playerNumber = 0;
    PlayerStateManager playerStateManager = new PlayerStateManager();
    //ステートの列挙型(初期ステートをセットするためのもの)
    enum PlayerStateEnum { In, Out };
    [SerializeField, Tooltip("プレイヤーの初期ステート")]
    PlayerStateEnum initStateEnum = PlayerStateEnum.In;

    Quaternion rotation = Quaternion.identity;

    float invincibleTimeCount = 0.0f;
    const float InvincibleTime = 3.0f;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        materialFlash = GetComponent<MaterialFlash>();
    }

    void Start()
    {
        PlayerStateBase initState = null;
        switch (initStateEnum)
        {
            case PlayerStateEnum.In:
                initState = new InBallState();
                break;
            case PlayerStateEnum.Out:
                initState = new OutBallState();
                break;
        }
        playerStateManager.Init(this, initState);
    }

    void Update()
    {
        invincibleTimeCount -= Time.deltaTime;
        if (IsInvincible())
        {
            materialFlash.FlashStart();
        }
        else
        {
            materialFlash.FlashEnd();
        }
        playerStateManager.Update();
    }

    void LateUpdate()
    {
        transform.rotation = rotation;
    }

    void OnDestroy()
    {
        playerStateManager.Destroy();
    }

    void OnCollisionEnter(Collision other)
    {
        //マップ外に出た時の処理
        if (other.gameObject.tag == "BreakArea")
        {
            PointManager.DropPlayerPointCalculate(playerNumber);
            playerStateManager.TranslationState(new RespawnState());
        }
        playerStateManager.OnCollisionEnter(other);
    }
    void OnCollisionStay(Collision other) { playerStateManager.OnCollisionStay(other); }
    void OnCollisionExit(Collision other) { playerStateManager.OnCollisionExit(other); }
    void OnTriggerEnter(Collider other) { playerStateManager.OnTriggerEnter(other); }
    void OnTriggerStay(Collider other) { playerStateManager.OnTriggerStay(other); }
    void OnTriggerExit(Collider other) { playerStateManager.OnTriggerExit(other); }

    /// <summary>
    /// プレイヤーの回転
    /// </summary>
    void PlayerRotation(Vector3 lookatDir)
    {
        if (lookatDir == Vector3.zero) return;
        //プレイヤーの回転
        var obj = new GameObject("");
        obj.transform.LookAt(Vector3.Cross(transform.right, Vector3.up));
        var startQ = obj.transform.rotation;
        obj.transform.LookAt(lookatDir);
        var endQ = obj.transform.rotation;
        rotation = Quaternion.Lerp(startQ, endQ, 0.3f);
        Destroy(obj);
    }

    /// <summary>
    /// プレイヤーの番号を取得
    /// </summary>
    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    /// <summary>
    /// 無敵中かどうか
    /// </summary>
    public bool IsInvincible()
    {
        return invincibleTimeCount > 0.0f;
    }

    /// <summary>
    /// プレイヤーが入っているボールにぶつかった
    /// </summary>
    public void HitInPlayerBall()
    {
        playerStateManager.TranslationState(new HitInPlayerBallState());
    }
}
