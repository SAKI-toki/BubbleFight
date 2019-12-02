using UnityEngine;

/// <summary>
/// ステージセレクトの穴の当たり判定
/// </summary>
public class StageSelectHoleCollider : MonoBehaviour
{
    [SerializeField, Tooltip("ステージの番号")]
    int stageNumber = 0;

    StageSelectVoting.AddVotingFunctionType collisionPlayerEvent;

    /// <summary>
    /// ステージの番号を取得
    /// </summary>
    public int GetStageNumber()
    {
        return stageNumber;
    }

    /// <summary>
    /// イベントのセット
    /// </summary>
    public void SetEvent(StageSelectVoting.AddVotingFunctionType function)
    {
        collisionPlayerEvent = function;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            var ballBehaviour = other.gameObject.GetComponent<BallBehaviour>();
            collisionPlayerEvent(ballBehaviour.GetPlayerIndex());
            Destroy(other.gameObject);
        }
    }
}