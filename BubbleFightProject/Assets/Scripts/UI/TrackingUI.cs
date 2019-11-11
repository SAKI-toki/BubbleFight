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
    float globalOffsetY = 0.0f;

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
            Vector3.Cross(Camera.main.transform.forward, Camera.main.transform.right) * globalOffsetY);
    }
}
