using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ選択の投票
/// </summary>
public class StageSelectVoting : MonoBehaviour
{
    [SerializeField, Tooltip("穴の当たり判定")]
    StageSelectHoleCollider[] stageSelectHoleColliders = null;

    //イベントの型
    public delegate void AddVotingFunctionType();
    //投票数
    int[] votingCounts;

    [SerializeField, Tooltip("プレイヤーの生成位置")]
    Transform[] playerGenerateTransforms = null;

    void Start()
    {
        //全ての番号があるか確認する
        if (!HasAllNumber()) return;
        //配列の要素数の確保
        votingCounts = new int[stageSelectHoleColliders.Length];
        //全ての穴の当たり判定にイベントを追加する
        foreach (var stageSelectHoleCollider in stageSelectHoleColliders)
        {
            stageSelectHoleCollider.SetEvent(delegate { votingCounts[stageSelectHoleCollider.GetStageNumber()] += 1; });
        }
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (PlayerJoinManager.IsJoin(i))
            {
                //プレイヤー生成
            }
        }
    }

    void Update()
    {
        if (AlreadyAllPlayerVoting())
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    /// <summary>
    /// 全てのプレイヤーが投票したかどうか
    /// </summary>
    bool AlreadyAllPlayerVoting()
    {
        int votingSum = 0;
        foreach (var votingCount in votingCounts)
        {
            votingSum += votingCount;
        }
        return votingSum == PlayerJoinManager.GetJoinPlayerCount();
    }

    /// <summary>
    /// 全ての番号があるかどうか
    /// </summary>
    bool HasAllNumber()
    {
        bool[] alreadyAppearNumber = new bool[stageSelectHoleColliders.Length];
        foreach (var stageSelectHoleCollider in stageSelectHoleColliders)
        {
            var stageNumber = stageSelectHoleCollider.GetStageNumber();
            if (alreadyAppearNumber[stageNumber])
            {
                Debug.LogError(stageNumber.ToString() + "のステージ番号が複数あります");
                return false;
            }
            alreadyAppearNumber[stageNumber] = true;
        }
        return true;
    }
}