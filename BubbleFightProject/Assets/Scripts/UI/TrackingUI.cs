using UnityEngine;

/// <summary>
/// UIの追尾
/// </summary>
public class TrackingUI : MonoBehaviour
{
    [SerializeField, Tooltip("追尾するターゲットのTransform")]
    Transform targetTransform = null;
    [SerializeField, Tooltip("追尾させるUIのRectTransform")]
    RectTransform rectTransform = null;
    [SerializeField, Tooltip("オフセット")]
    Vector2 globalOffset = Vector2.zero;

    void LateUpdate()
    {
        if (!targetTransform)
        {
            Destroy(this.gameObject);
            return;
        }
        //追尾
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(
            Camera.main, targetTransform.position +
            Camera.main.transform.right * globalOffset.x +
            Camera.main.transform.up * globalOffset.y);
    }
}
