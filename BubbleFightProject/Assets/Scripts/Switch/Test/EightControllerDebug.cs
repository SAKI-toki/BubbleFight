using UnityEngine;

public class EightControllerDebug : MonoBehaviour
{
    [SerializeField]
    RectTransform[] rectTransforms = null;

    [SerializeField]
    float moveSpeed = 1.0f;

    const int ControllerCount = 8;

    void Start()
    {
        Debug.Assert(rectTransforms.Length == ControllerCount);
    }

    void Update()
    {
        for (int i = 0; i < ControllerCount; ++i)
        {
            float hor = SwitchInput.GetHorizontal(i);
            float ver = SwitchInput.GetVertical(i);

            var pos = rectTransforms[i].localPosition;
            pos.x += hor * moveSpeed * Time.deltaTime;
            pos.y += ver * moveSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -340, 340);
            pos.y = Mathf.Clamp(pos.y, -200, 200);
            rectTransforms[i].localPosition = pos;
        }
    }
}
