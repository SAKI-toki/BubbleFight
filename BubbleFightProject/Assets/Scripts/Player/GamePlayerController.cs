using UnityEngine;

/// <summary>
/// プレイヤーの制御クラス
/// </summary>
public partial class GamePlayerController : PlayerBehaviour
{
    protected override void PlayerAwake()
    {
        materialFlash = GetComponent<MaterialFlash>();
        inBallModelLocalPositionY = modelTransform.localPosition.y;
    }

    protected override void PlayerStart()
    {
        playerStateManager.Init(this, new GameIntoBallState());
    }

    protected override void PlayerUpdate()
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
    }

    protected override void PlayerOnCollisionEnter(Collision other)
    {
        //マップ外に出た時の処理
        if (other.gameObject.tag == "BreakArea")
        {
            CollisionBreakArea();
        }
    }


    /// <summary>
    /// ボールが壊れたときの処理
    /// </summary>
    public void CollisionBreakArea()
    {
        if (alreadyCollisionBreakArea) return;
        alreadyCollisionBreakArea = true;
        PointManager.DropPlayerPointCalculate(playerNumber);
        playerStateManager.TranslationState(new GameRespawnState());
    }
}