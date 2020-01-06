using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// </summary>
public partial class GameBallController : BallBehaviour
{
    const int HitPlayerNumberLength = 8;
    //当たった番号を保持
    int[] hitPlayerNumbers = new int[HitPlayerNumberLength];

    protected override void BallStart()
    {
        for (int i = 0; i < HitPlayerNumberLength; ++i)
        {
            hitPlayerNumbers[i] = int.MaxValue;
        }
        ballStateManager.Init(this, new GameNotHasPlayerState());
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

        var otherGameBallController = other.gameObject.GetComponent<GameBallController>();

        //ダメージ(空の場合は10分の1のダメージにする)
        currentHitPoint -= DamageCalculate(other.relativeVelocity.sqrMagnitude, otherGameBallController.prevVelocity.sqrMagnitude)
                            * (otherGameBallController.IsInPlayer() ? 1.0f : 0.1f);

        //最後にぶつかったプレイヤーの更新
        LastHitPlayerManager.SetLastHitPlayer(GetPlayerIndex(), otherGameBallController.GetPlayerIndex());

        //HPが0以下になったら破壊
        if (currentHitPoint <= 0)
        {
            //PointManager.BreakBallPointCalculate(otherGameBallController, this);
            BrokenBall();
        }

        //プレイヤーの番号を格納
        if (otherGameBallController.IsInPlayer())
        {
            for (int i = HitPlayerNumberLength - 2; i >= 0; --i)
            {
                hitPlayerNumbers[i] = hitPlayerNumbers[i + 1];
            }
            hitPlayerNumbers[0] = otherGameBallController.GetPlayerIndex();
        }
    }

    /// <summary>
    /// ゴールに入った時の処理
    /// </summary>
    void CollisionGoal(Collision other)
    {
        var goalController = other.gameObject.GetComponent<GoalController>();
        int goalNumber = goalController.GetGoalNumber();
        foreach (var hitPlayerNumber in hitPlayerNumbers)
        {
            if (hitPlayerNumber == goalNumber) continue;

            //オウンゴール
            if (hitPlayerNumber == int.MaxValue)
            {
                PointManager.OwnGoalCalculate(goalNumber);
            }
            //普通のゴール
            else
            {
                PointManager.GoalCalculate(goalNumber, hitPlayerNumber);
            }
            break;
        }
        BrokenBall();
    }
}