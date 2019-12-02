public class StageVotingPlayerController : PlayerBehaviour
{
    protected override void PlayerAwake()
    {
        inBallModelLocalPositionY = modelTransform.localPosition.y;
    }

    protected override void PlayerStart()
    {
        playerStateManager.Init(this, new StageVotingInBallState());
    }
}