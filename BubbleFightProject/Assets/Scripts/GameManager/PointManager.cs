/// <summary>
/// ポイントを管理するクラス
/// </summary>
static public class PointManager
{
    //プレイヤーのポイント
    static int[] playerPoint;

    static bool pointLock = false;

    //ゴールのポイント
    const int GoalPoint = 1;

    public delegate void AddPointFunctionType(int point, int addPoint);
    public static AddPointFunctionType AddPointFunction;

    /// <summary>
    /// ゴール時のポイント計算
    /// </summary>
    static public void GoalCalculate(int goalNumber, int playerNumber)
    {
        AddPoint(playerNumber, GoalPoint);
        AddPoint(goalNumber, -GoalPoint);
    }

    /// <summary>
    /// オウンゴール時のポイント計算
    /// </summary>
    static public void OwnGoalCalculate(int playerNumber)
    {
        AddPoint(playerNumber, -GoalPoint);
    }


    /// <summary>
    /// ポイントの加算
    /// </summary>
    static void AddPoint(int index, int addPoint)
    {
        if (pointLock) return;
        playerPoint[index] += addPoint;
        if (AddPointFunction != null) AddPointFunction(index, addPoint);
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