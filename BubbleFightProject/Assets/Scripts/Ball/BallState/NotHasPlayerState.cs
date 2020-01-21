using UnityEngine;

public partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っていないステート
    /// </summary>
    protected class NotHasPlayerState : BallStateBase
    {
        public override void OnCollisionEnter(Collision other)
        {
            switch (other.gameObject.tag)
            {
                case "Goal":
                    {
                        //入れたゴールの番号を取得
                        int goalNumber = other.gameObject.GetComponent<GoalController>().GetGoalNumber();
                        if (!PlayerJoinManager.IsJoin(goalNumber) || PointManager.GetPoint(goalNumber) <= 0) return;

                        if (ballBehaviour.isColor && ballBehaviour.playerNumberByColor == goalNumber)
                        {
                            PointManager.ByColorGoalCalculate(goalNumber);
                        }
                        else
                        {
                            PointManager.GoalCalculate(goalNumber);
                        }

                        GameObject.Destroy(ballBehaviour.gameObject);
                    }
                    break;
            }
        }
    }
}
