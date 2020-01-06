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
}