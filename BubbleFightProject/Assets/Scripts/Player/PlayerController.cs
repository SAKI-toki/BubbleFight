using UnityEngine;

/// <summary>
/// プレイヤーの制御クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MaterialFlash))]
[RequireComponent(typeof(PlayerAnimationController))]
public partial class PlayerController : MonoBehaviour
{
    Rigidbody playerRigidbody = null;
    MaterialFlash materialFlash = null;
    PlayerAnimationController playerAnimation = null;
    PlayerGenerator playerGenerator = null;
    //プレイヤーの番号
    int playerNumber = 0;
    PlayerStateManager playerStateManager = new PlayerStateManager();
    //ステートの列挙型(初期ステートをセットするためのもの)
    public enum PlayerStateEnum { In, Out };
    PlayerStateEnum initStateEnum = PlayerStateEnum.Out;
    Quaternion rotation = Quaternion.identity;
    float invincibleTimeCount = 0.0f;
    //無敵時間
    const float InvincibleTime = 3.0f;

    [SerializeField, Tooltip("三人称視点カメラ")]
    GameObject cameraObject = null;
    [SerializeField, Tooltip("カメラの注視点")]
    Transform cameraLookat = null;
    TpsCamera cameraController = null;
    [SerializeField, Tooltip("モデルのローカルTransform")]
    Transform modelTransform = null;
    float inBallModelLocalPositionY = 0.0f;

    PlayerTypeStatusScriptableObject status;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        materialFlash = GetComponent<MaterialFlash>();
        playerAnimation = GetComponent<PlayerAnimationController>();
        inBallModelLocalPositionY = modelTransform.localPosition.y;
    }

    void Start()
    {
        //ステータスを取得
        status = PlayerTypeManager.GetInstance().GetPlayerStatus(playerNumber);
        rotation = transform.rotation;
        cameraController = Instantiate(cameraObject).GetComponent<TpsCamera>();
        cameraController.CameraInit(playerNumber, cameraLookat);
        PlayerStateBase initState = null;
        switch (initStateEnum)
        {
            case PlayerStateEnum.In:
                initState = new IntoBallState();
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
        playerStateManager.LateUpdate();
        //回転をセットした後に実行しなければならない
        cameraController.CameraUpdate();

        var velocity = playerRigidbody.velocity;
        velocity.x = velocity.z = 0;
        playerRigidbody.velocity = velocity;
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
        rotation = Quaternion.Lerp(startQ, endQ, 10 * Time.deltaTime);
        Destroy(obj);
    }

    /// <summary>
    /// プレイヤーの物理演算のオンオフ
    /// </summary>
    void PhysicsSet(bool enabled)
    {
        //コライダーのオンオフ
        foreach (var collider in GetComponents<Collider>()) collider.enabled = enabled;
        foreach (var collider in GetComponentsInChildren<Collider>()) collider.enabled = enabled;

        if (enabled)
        {
            //この順番にしないと警告が出る
            playerRigidbody.isKinematic = false;
            playerRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        else
        {
            //この順番にしないと警告が出る
            playerRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            playerRigidbody.isKinematic = true;
        }
    }

    /// <summary>
    /// 前移動の方向をカメラから取得
    /// </summary>
    Vector3 GetMoveForwardDirection()
    {
        return cameraController.GetMoveForwardDirection();
    }


    /// <summary>
    /// 右移動の方向をカメラから取得
    /// </summary>
    Vector3 GetMoveRightDirection()
    {
        return cameraController.GetMoveRightDirection();
    }

    /// <summary>
    /// /// プレイヤーの番号を取得
    /// </summary>
    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    /// <summary>
    /// プレイヤーの番号をセット
    /// </summary>
    public void SetPlayerNumber(int number)
    {
        playerNumber = number;
    }

    /// <summary>
    /// 初期ステートをボールの中からにする
    /// </summary>
    public void SetInitState(PlayerStateEnum state)
    {
        initStateEnum = state;
    }

    /// <summary>
    /// プレイヤーの生成器をセット
    /// </summary>
    public void SetPlayerGenerator(PlayerGenerator generator)
    {
        playerGenerator = generator;
    }

    /// <summary>
    /// 無敵中かどうか
    /// </summary>
    public bool IsInvincible()
    {
        return invincibleTimeCount > 0.0f;
    }
}

/// <summary>
/// プレイヤーの計算
/// </summary>
static public class PlayerMath
{
    /// <summary>
    /// 前方向と右方向のベクトルから移動する向きを出力する
    /// </summary>
    static public Vector3 ForwardAndRightMove(Vector2 globalDir, Vector3 forward, Vector3 right)
    {
        return globalDir.y * forward + globalDir.x * right;
    }

    /// <summary>
    /// 前方向と右方向のベクトルから移動する向きを出力する
    /// </summary>
    static public Vector3 ForwardAndRightMove(Vector3 globalDir, Vector3 forward, Vector3 right)
    {
        return globalDir.y * forward + globalDir.x * right;
    }
}

/// <summary>
/// プレイヤーの色
/// </summary>
static public class PlayerColor
{
    static Color[] color = new Color[4]
{
    Color.red,Color.blue,Color.yellow,Color.green
};

    /// <summary>
    /// 色の取得
    /// </summary>
    static public Color GetColor(int index)
    {
        return color[index];
    }

}