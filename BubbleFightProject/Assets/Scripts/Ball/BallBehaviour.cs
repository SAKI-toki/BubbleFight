﻿using UnityEngine;

/// <summary>
/// ボールの動作
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public partial class BallBehaviour : MonoBehaviour
{
    Rigidbody thisRigidbody;
    Vector3 lookatDir = Vector3.zero;

    //操作するプレイヤー
    int playerIndex = int.MaxValue;

    //当たる前の力を保持する変数
    Vector3 prevVelocity = Vector3.zero;

    BallStateManager ballStateManager = new BallStateManager();

    //入力を受け付けない時間
    float cantInputTime = 0.0f;

    [SerializeField, Tooltip("ボールの情報")]
    BallScriptableObject ballScriptableObject = null;
    Vector3 initPosition = Vector3.zero;
    Quaternion initRotation = Quaternion.identity;
    Transform playerTransform = null;
    Quaternion playerRotation = Quaternion.identity;
    GameObject playerRotationObject = null;
    PlayerAnimationController playerAnimationController = null;
    public delegate void DestroyEvent();
    DestroyEvent destroyEvent = null;

    float movePower, easyCurveWeight, mass, boostPower, boostInterval, brakePower;

    int destroyCount = 0;

    PlayerAnimationController uiPlayerAnim = null;

    [SerializeField]
    bool isColor = false;
    [SerializeField]
    int playerNumberByColor = 0;

    RectTransform numberUiTransform = null;

    [SerializeField]
    AudioClip brakeClip = null, hitClip = null;

    AudioSource brakeSound = null, hitSound = null;

    [SerializeField, Range(0, 1)]
    float hitSoundVolume = 0.0f, brakeSoundVolume = 0.0f;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        brakeSound = gameObject.AddComponent<AudioSource>();
        brakeSound.loop = false;
        brakeSound.playOnAwake = false;
        brakeSound.clip = brakeClip;
        brakeSound.volume = brakeSoundVolume;
        hitSound = gameObject.AddComponent<AudioSource>();
        hitSound.loop = false;
        hitSound.playOnAwake = false;
        hitSound.clip = hitClip;
    }

    void Start()
    {
        playerRotation = transform.rotation;
        thisRigidbody.maxAngularVelocity = ballScriptableObject.MaxAngularVelocity;
        if (IsInPlayer())
        {
            //プレイヤーの情報を格納
            var playerStatus = PlayerTypeManager.GetInstance().GetPlayerStatus(playerIndex);
            if (playerStatus.Type == PlayerType.Alpaca)
            {
                ((AlpacaStatus)playerStatus).AlpacaStatusInit(
                    ref movePower, ref easyCurveWeight, ref mass, ref boostPower, ref boostInterval, ref brakePower);
            }
            else
            {
                movePower = playerStatus.BallMovePower;
                easyCurveWeight = playerStatus.BallEasyCurveWeight;
                mass = playerStatus.BallMass;
                boostPower = playerStatus.BallBoostPower;
                boostInterval = playerStatus.BallBoostInterval;
                brakePower = playerStatus.BallBrakePower;
            }
            transform.GetComponent<SphereCollider>().material = playerStatus.BallPhysicalMaterial;
            thisRigidbody.mass = mass;
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
            playerTransform = playerAnimationController.transform;
            initPosition = transform.position;
            initRotation = transform.rotation;

            //色をプレイヤーの色に変える
            var mat = transform.GetComponent<MeshRenderer>().material;
            var color = PlayerColor.GetColor(playerIndex);
            mat.SetColor("_ColorDown", color);

            //回転しやすいように空のオブジェクトを作成
            playerRotationObject = new GameObject("PlayerRotationObject");
            playerRotationObject.transform.parent = transform;
            playerTransform.localPosition = Vector3.zero;

            ballStateManager.Init(this, new HasPlayerState());
        }
        else
        {
            ballStateManager.Init(this, new NotHasPlayerState());
        }
    }

    void Update()
    {
        if (Time.timeScale == 0.0f || Fade.instance.IsFade) return;
        prevVelocity = thisRigidbody.velocity;
        thisRigidbody.AddForce(Vector3.up * -ballScriptableObject.Gravity);
        ballStateManager.Update();
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0.0f || Fade.instance.IsFade) return;
        RestrictVelocity();
        ballStateManager.LateUpdate();
        if (IsInPlayer())
        {
            playerTransform.rotation = playerRotation;
        }

    }

    void OnDestroy()
    {
        ballStateManager.Destroy();
        if (destroyEvent != null) destroyEvent();
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
        }
        ballStateManager.OnCollisionEnter(other);
    }
    void OnCollisionStay(Collision other) { ballStateManager.OnCollisionStay(other); }
    void OnCollisionExit(Collision other) { ballStateManager.OnCollisionExit(other); }
    void OnTriggerEnter(Collider other) { ballStateManager.OnTriggerEnter(other); }
    void OnTriggerStay(Collider other) { ballStateManager.OnTriggerStay(other); }
    void OnTriggerExit(Collider other) { ballStateManager.OnTriggerExit(other); }

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

    /// <summary>
    /// 破棄時のイベントをセット
    /// </summary>
    public void SetDestroyEvent(DestroyEvent argDestroyEvent)
    {
        destroyEvent = argDestroyEvent;
    }

    public void SetNumberUI(RectTransform rectTransform)
    {
        numberUiTransform = rectTransform;
    }
    public void SetUiPlayerAnim(PlayerAnimationController anim)
    {
        uiPlayerAnim = anim;
    }

    /// <summary>
    /// プレイヤーの回転
    /// </summary>
    void PlayerRotation(Vector3 lookatDir)
    {
        if (lookatDir == Vector3.zero) return;

        playerRotationObject.transform.position = Vector3.zero;
        playerRotationObject.transform.LookAt(Vector3.Cross(playerTransform.right, Vector3.up));
        var startQ = playerRotationObject.transform.rotation;
        playerRotationObject.transform.LookAt(lookatDir);
        var endQ = playerRotationObject.transform.rotation;
        playerRotation = Quaternion.Lerp(startQ, endQ, 10 * Time.deltaTime);
    }

    /// <summary>
    /// ボールとの衝突
    /// </summary>
    void CollisionBall(Collision other)
    {
        //跳ね返りの強さ
        float bounceAddPower = other.relativeVelocity.sqrMagnitude > ballScriptableObject.CantInputHitPower ?
                                ballScriptableObject.StrongHitBounceAddPower : ballScriptableObject.WeakHitBounceAddPower;
        var velocity = thisRigidbody.velocity;
        velocity.x *= bounceAddPower;
        velocity.z *= bounceAddPower;
        thisRigidbody.velocity = velocity;
        if (other.relativeVelocity.magnitude > 8.0f)
        {
            float volume = Mathf.Clamp(other.relativeVelocity.magnitude / 25.0f, 0.1f, 1.0f);
            hitSound.volume = volume * hitSoundVolume;
            if (volume >= 1.0f) CameraShake.Shake(0.2f);
            hitSound.Play();
        }
    }
}
