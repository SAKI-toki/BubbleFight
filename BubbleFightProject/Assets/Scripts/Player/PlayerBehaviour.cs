using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MaterialFlash))]
[RequireComponent(typeof(PlayerAnimationController))]
public abstract partial class PlayerBehaviour : MonoBehaviour
{
    protected Rigidbody playerRigidbody = null;
    protected MaterialFlash materialFlash = null;
    protected PlayerAnimationController playerAnimation = null;
    PlayerGenerator playerGenerator = null;
    //プレイヤーの番号
    protected int playerNumber = 0;
    protected PlayerStateManager playerStateManager = new PlayerStateManager();
    protected Quaternion rotation = Quaternion.identity;

    [SerializeField, Tooltip("三人称視点カメラ")]
    protected GameObject cameraObject = null;
    [SerializeField, Tooltip("カメラの注視点")]
    protected Transform cameraLookat = null;
    protected TpsCamera cameraController = null;
    protected PlayerTypeStatusScriptableObject status;
    protected float invincibleTimeCount = 0.0f;
    //無敵時間
    protected const float InvincibleTime = 3.0f;
    [SerializeField, Tooltip("モデルのローカルTransform")]
    protected Transform modelTransform = null;
    protected float inBallModelLocalPositionY = 0.0f;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimationController>();
        PlayerAwake();
    }

    void Start()
    {
        //ステータスを取得
        status = PlayerTypeManager.GetInstance().GetPlayerStatus(playerNumber);
        rotation = transform.rotation;
        cameraController = Instantiate(cameraObject).GetComponent<TpsCamera>();
        cameraController.CameraInit(playerNumber, cameraLookat);
        PlayerStart();
    }

    void Update()
    {
        playerRigidbody.AddForce(Vector3.up * -10);
        PlayerUpdate();
        playerStateManager.Update();
    }

    void LateUpdate()
    {
        PlayerLateUpdate();

        playerStateManager.LateUpdate();
        transform.rotation = rotation;
        //回転をセットした後に実行しなければならない
        cameraController.CameraUpdate();

        var velocity = playerRigidbody.velocity;
        velocity.x = velocity.z = 0;
        playerRigidbody.velocity = velocity;
    }

    void OnDestroy()
    {
        PlayerOnDestroy();
        playerStateManager.Destroy();
    }

    void OnCollisionEnter(Collision other) { PlayerOnCollisionEnter(other); playerStateManager.OnCollisionEnter(other); }
    void OnCollisionStay(Collision other) { PlayerOnCollisionStay(other); playerStateManager.OnCollisionStay(other); }
    void OnCollisionExit(Collision other) { PlayerOnCollisionExit(other); playerStateManager.OnCollisionExit(other); }
    void OnTriggerEnter(Collider other) { PlayerOnTriggerEnter(other); playerStateManager.OnTriggerEnter(other); }
    void OnTriggerStay(Collider other) { PlayerOnTriggerStay(other); playerStateManager.OnTriggerStay(other); }
    void OnTriggerExit(Collider other) { PlayerOnTriggerExit(other); playerStateManager.OnTriggerExit(other); }

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
    /// 無敵中かどうか
    /// </summary>
    public bool IsInvincible()
    {
        return invincibleTimeCount > 0.0f;
    }

    /// <summary>
    /// プレイヤーの生成器をセット
    /// </summary>
    public void SetPlayerGenerator(PlayerGenerator generator)
    {
        playerGenerator = generator;
    }

    protected virtual void PlayerAwake() { }
    protected virtual void PlayerStart() { }
    protected virtual void PlayerUpdate() { }
    protected virtual void PlayerLateUpdate() { }
    protected virtual void PlayerOnDestroy() { }
    protected virtual void PlayerOnCollisionEnter(Collision other) { }
    protected virtual void PlayerOnCollisionStay(Collision other) { }
    protected virtual void PlayerOnCollisionExit(Collision other) { }
    protected virtual void PlayerOnTriggerEnter(Collider other) { }
    protected virtual void PlayerOnTriggerStay(Collider other) { }
    protected virtual void PlayerOnTriggerExit(Collider other) { }
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