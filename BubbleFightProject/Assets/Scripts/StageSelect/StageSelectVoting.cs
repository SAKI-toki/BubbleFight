using System.Collections;
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
    public delegate void AddVotingFunctionType(int playerIndex);
    //投票数
    int[] votingCounts;

    [SerializeField, Tooltip("プレイヤーの生成位置")]
    Transform[] playerGenerateTransforms = null;

    [SerializeField, Tooltip("ボールのプレファブ")]
    GameObject ballPrefab = null;

    void Start()
    {
        //全ての番号があるか確認する
        if (!HasAllNumber()) return;
        //配列の要素数の確保
        votingCounts = new int[stageSelectHoleColliders.Length];
        //全ての穴の当たり判定にイベントを追加する
        foreach (var stageSelectHoleCollider in stageSelectHoleColliders)
        {
            stageSelectHoleCollider.SetEvent(
                delegate (int playerIndex)
                {
                    votingCounts[stageSelectHoleCollider.GetStageNumber()] += 1;
                    StartCoroutine(FadeCamera(playerIndex));
                });
        }
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (PlayerJoinManager.IsJoin(i))
            {
                //プレイヤー生成
                var player = PlayerTypeManager.GetInstance().GeneratePlayer(i, PlayerTypeManager.SceneType.StageVoting);
                var playerBehaviour = player.GetComponent<PlayerBehaviour>();
                //番号をセット
                playerBehaviour.SetPlayerNumber(i);
                //ボールの生成
                var ball = Instantiate(ballPrefab, playerGenerateTransforms[i].position, playerGenerateTransforms[i].rotation);
                player.transform.parent = ball.transform;
                player.transform.localPosition = Vector3.zero;
                player.transform.localRotation = Quaternion.identity;
            }
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

    /// <summary>
    /// カメラのフェード
    /// </summary>
    IEnumerator FadeCamera(int index)
    {
        if (AlreadyAllPlayerVoting())
        {
            var postprocess = Camera.main.GetComponent<FadePostprocess>();
            float percent = 0.0f;
            while (percent < 1.0f)
            {
                percent += Time.deltaTime / 2;
                postprocess.SetValue(percent);
                yield return null;
            }
            SceneManager.LoadScene("GameScene");
        }
    }
}
