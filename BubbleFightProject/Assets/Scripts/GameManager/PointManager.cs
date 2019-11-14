/// <summary>
/// ポイントを管理するクラス
/// </summary>
static public class PointManager
{
    //プレイヤーのポイント
    static int[] playerPoint;

    static bool pointLock = false;

    //ボールを壊した時のポイント
    const int BreakBallPoint = 1;
    //ボールを壊された時のポイント
    const int BrokenBallPoint = -1;
    //プレイヤーを壊した時のポイント
    const int BreakPlayerPoint = 2;
    //プレイヤーを壊された時のポイント
    const int BrokenPlayerPoint = -2;
    //場外に落とした時のポイント
    const int DropPoint = 3;
    //場外に落とされた時のポイント
    const int DroppedPoint = -3;

    /// <summary>
    /// ボールを壊した時のポイントの計算
    /// </summary>
    static public void BreakBallPointCalculate(BallController breaker, BallController broken)
    {
        if (broken.IsInPlayer())
        {
            AddPoint(broken.GetPlayerIndex(), BrokenBallPoint);
            if (breaker.IsInPlayer())
            {
                AddPoint(breaker.GetPlayerIndex(), BreakBallPoint);
            }
        }
    }

    /// <summary>
    /// プレイヤーを壊した時のポイントの計算
    /// </summary>
    static public void BreakPlayerPointCalculate(BallController breaker, PlayerController playerController)
    {
        if (breaker.IsInPlayer())
        {
            AddPoint(breaker.GetPlayerIndex(), BreakPlayerPoint);
            AddPoint(playerController.GetPlayerNumber(), BrokenPlayerPoint);
        }
    }

    /// <summary>
    /// 場外に落とした時のポイントの計算
    /// </summary>
    static public void DropPlayerPointCalculate(int broken)
    {

        if (broken != int.MaxValue)
        {
            AddPoint(broken, DroppedPoint);
            if (LastHitPlayerManager.IsValid(broken))
            {
                AddPoint(LastHitPlayerManager.GetLastHitPlayer(broken), DropPoint);
                LastHitPlayerManager.UseLastHitPlayer(broken);
            }
        }
    }


    /// <summary>
    /// ポイントの加算
    /// </summary>
    static public void AddPoint(int index, int addPoint)
    {
        if (pointLock) return;
        playerPoint[index] += addPoint;
    }

    //ポイントの取得
    static public int GetPoint(int index)
    {
        return playerPoint[index];
    }
    /// <summary>
    /// ポイントのリセット
    /// </summary>
    static public void Reset()
    {
        playerPoint = new int[PlayerJoinManager.GetJoinPlayerCount()];
        pointLock = false;
    }

    /// <summary>
    /// ポイントをロックする
    /// </summary>
    static public void PointLock()
    {
        pointLock = true;
    }

}