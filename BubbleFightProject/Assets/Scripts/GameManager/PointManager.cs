﻿/// <summary>
/// ポイントを管理するクラス
/// </summary>
static public class PointManager
{
    //プレイヤーのポイント
    static int[] playerPoints;
    //順位
    static int[] playerRanks;

    static bool pointLock = false;

    //始まりのポイント
    const int StartPoint = 20;

    //ゴールのポイント
    const int GoalPoint = 1;
    //自分のゴールに入ったときのポイント
    const int OwnGoalPoint = 3;
    //自分の色がゴールに入ったときのポイント
    const int ByColorGoalPoint = 3;

    /// <summary>
    /// ゴール時のポイント計算
    /// </summary>
    static public void GoalCalculate(int goalNumber)
    {
        if (pointLock) return;
        if (playerPoints[goalNumber] <= 0) return;
        playerPoints[goalNumber] -= GoalPoint;
        GoalCalculateImpl(goalNumber);
    }

    static public void OwnGoalCalculate(int playerNumber)
    {
        if (pointLock) return;
        if (playerPoints[playerNumber] <= 0) return;
        playerPoints[playerNumber] -= OwnGoalPoint;
        GoalCalculateImpl(playerNumber);
    }
    static public void ByColorGoalCalculate(int playerNumber)
    {
        if (pointLock) return;
        if (playerPoints[playerNumber] <= 0) return;
        playerPoints[playerNumber] -= ByColorGoalPoint;
        GoalCalculateImpl(playerNumber);
    }


    static void GoalCalculateImpl(int n)
    {
        //ポイントが0になったら
        if (playerPoints[n] <= 0)
        {
            playerPoints[n] = 0;
            int currentRank = 0;
            for (int i = 0; i < PlayerCount.MaxValue; ++i)
            {
                if (!PlayerJoinManager.IsJoin(i)) continue;
                if (playerRanks[i] == 0) ++currentRank;
            }
            playerRanks[n] = currentRank;
        }
    }

    /// <summary>
    /// ポイントの取得
    /// </summary>
    static public int GetPoint(int index)
    {
        return playerPoints[index];
    }

    /// <summary>
    /// 順位の取得
    /// </summary>
    static public int GetRank(int index)
    {
        return playerRanks[index];
    }

    /// <summary>
    /// ポイントのリセット
    /// </summary>
    static public void Reset()
    {
        playerPoints = new int[PlayerCount.MaxValue];
        playerRanks = new int[PlayerCount.MaxValue];

        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            playerPoints[i] = StartPoint;
        }
        pointLock = false;
    }

    /// <summary>
    /// ポイントをロックする
    /// </summary>
    static public void PointLock()
    {
        pointLock = true;
    }

    /// <summary>
    /// 順位を出す
    /// </summary>
    static public void ApplyRank()
    {
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (!PlayerJoinManager.IsJoin(i)) continue;
            if (playerRanks[i] != 0) continue;
            int rank = 1;
            for (int j = 0; j < PlayerCount.MaxValue; ++j)
            {
                if (!PlayerJoinManager.IsJoin(j)) continue;
                if (playerPoints[i] < playerPoints[j]) ++rank;
            }
            playerRanks[i] = rank;
        }
    }
}
