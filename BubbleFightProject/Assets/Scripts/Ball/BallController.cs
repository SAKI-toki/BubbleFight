﻿using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BallController : MonoBehaviour
{
    Rigidbody thisRigidbody;
    Vector3 inputDir = Vector3.zero;
    Vector3 lookatDir = Vector3.zero;

    //操作するプレイヤー
    int playerIndex = int.MaxValue;
    //移動力
    float movePower = 0.0f;

    [SerializeField, Tooltip("ボールの耐久値")]
    float hitPoint = 100;

    [SerializeField, Tooltip("与えるダメージ量(指数的に増加)")]
    float damageWeight = 2.0f;
    //当たる前の力を保持する変数
    Vector3 prevVelocity = Vector3.zero;

    public delegate void DestroyEventType();
    DestroyEventType destroyEvent;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        prevVelocity = thisRigidbody.velocity;
    }

    /// <summary>
    /// プレイヤーによる初期化
    /// </summary>
    public void InitializeOnPlayer(int index, float ballMovePower, float ballMass)
    {
        playerIndex = index;
        movePower = ballMovePower;
        thisRigidbody.mass = ballMass;
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void Move(float easyCurveWeight)
    {
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
        //曲がるときの力
        float curvePower = angle + 1;
        //入力方向に力を加える
        thisRigidbody.AddForce(inputDir * movePower * thisRigidbody.mass * Mathf.Pow(curvePower, easyCurveWeight));

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
                                        (other.transform.childCount == 0 ? 0.1f : 1.0f);

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