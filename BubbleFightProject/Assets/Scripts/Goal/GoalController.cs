using UnityEngine;

/// <summary>
/// ゴールの制御クラス
/// </summary>
public class GoalController : MonoBehaviour
{
    [SerializeField, Tooltip("ゴールの番号")]
    int goalNumber = 0;
    Transform childTransform = null;
    bool zeroPointFlag = false;

    void Start()
    {
        childTransform = transform.GetChild(0);
        var mat = childTransform.GetComponent<MeshRenderer>().material;
        mat.color = PlayerColor.GetColor(goalNumber);
        childTransform.GetComponent<MeshRenderer>().material = mat;
    }

    void Update()
    {
        if (!zeroPointFlag && PointManager.GetPoint(goalNumber) <= 0)
        {
            zeroPointFlag = true;
            var scale = childTransform.localScale;
            scale.y = 4;
            childTransform.localScale = scale;
        }
    }

    /// <summary>
    /// ゴールの番号を取得
    /// </summary>
    public int GetGoalNumber()
    {
        return goalNumber;
    }
}