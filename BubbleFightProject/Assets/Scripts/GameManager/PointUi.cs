using UnityEngine;
using UnityEngine.UI;

public class PointUi : MonoBehaviour
{
    [SerializeField]
    int playerNumber = 0;

    [SerializeField]
    Image pointImage = null;

    [SerializeField]
    Sprite[] pointSprites = null;

    bool deathFlag = false;

    void Update()
    {
        if (deathFlag) return;
        pointImage.sprite = pointSprites[PointManager.GetPoint(playerNumber)];
        if (PointManager.GetPoint(playerNumber) == 0)
        {
            deathFlag = true;
            pointImage.rectTransform.localPosition = Vector3.zero;
            pointImage.rectTransform.localScale = pointImage.rectTransform.localScale * 1.5f;
        }
    }
}
