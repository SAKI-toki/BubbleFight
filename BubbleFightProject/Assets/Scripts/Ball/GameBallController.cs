using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// </summary>
public partial class GameBallController : BallBehaviour
{
    protected override void BallStart()
    {
        ballStateManager.Init(this, new GameNotHasPlayerState());
    }

    protected override void BallOnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Ball":
                CollisionBall(other);
                break;
            case "BreakArea":
                CollisionBreakArea();
                break;
        }
    }

    /// <summary>
    /// ボールとの衝突
    /// </summary>
    void CollisionBall(Collision other)
    {
        var otherGameBallController = other.gameObject.GetComponent<GameBallController>();
        //ダメージ(空の場合は10分の1のダメージにする)
        currentHitPoint -= DamageCalculate(other.relativeVelocity.sqrMagnitude, otherGameBallController.prevVelocity.sqrMagnitude)
                            * (otherGameBallController.IsInPlayer() ? 1.0f : 0.1f);

        //跳ね返りの強さ
        float bounceAddPower = other.relativeVelocity.sqrMagnitude > ballScriptableObject.CantInputHitPower ?
                                ballScriptableObject.StrongHitBounceAddPower : ballScriptableObject.WeakHitBounceAddPower;
        var velocity = thisRigidbody.velocity;
        velocity.x *= bounceAddPower;
        velocity.z *= bounceAddPower;
        thisRigidbody.velocity = velocity;

        //最後にぶつかったプレイヤーの更新
        LastHitPlayerManager.SetLastHitPlayer(GetPlayerIndex(), otherGameBallController.GetPlayerIndex());

        //HPが0以下になったら破壊
        if (currentHitPoint <= 0)
        {
            PointManager.BreakBallPointCalculate(otherGameBallController, this);
            BrokenBall();
        }
    }

    /// <summary>
    /// マップ外に出た時の処理
    /// </summary>
    void CollisionBreakArea()
    {
        var gamePlayerController = GetComponentInChildren<GamePlayerController>();
        gamePlayerController.CollisionBreakArea();
        BrokenBall();
    }
}