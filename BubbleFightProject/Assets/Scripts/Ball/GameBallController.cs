using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// </summary>
public partial class GameBallController : BallBehaviour
{
    protected override void BallStart()
    {
        if (IsInPlayer())
        {
            ballStateManager.Init(this, new GameHasPlayerState());
        }
        else
        {
            ballStateManager.Init(this, new GameNotHasPlayerState());
        }
    }

    protected override void BallOnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Ball":
                CollisionBall(other);
                break;
            case "Goal":
                CollisionGoal(other);
                break;
        }
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
    }

    /// <summary>
    /// ゴールに入った時の処理
    /// </summary>
    void CollisionGoal(Collision other)
    {
    }
}