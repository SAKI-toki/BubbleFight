using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ポイントの追加のUI
/// </summary>
public class AddPointUI : MonoBehaviour
{
    [SerializeField, Tooltip("時間")]
    float lifetime = 0.0f;
    [SerializeField, Tooltip("アニメーションさせるテキスト")]
    Text animationText = null;
    [SerializeField, Tooltip("スタート位置")]
    Vector3 startPoisition = Vector3.zero;
    [SerializeField, Tooltip("終了位置")]
    Vector3 endPoisition = Vector3.zero;
    [SerializeField, Tooltip("位置の曲線")]
    AnimationCurve positionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    [SerializeField, Tooltip("アルファ値の曲線")]
    AnimationCurve alphaCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    float lifetimeCount = 0.0f;
    RectTransform textRectTransform = null;

    void Awake()
    {
        var col = animationText.color;
        col.a = 0;
        animationText.color = col;
    }

    void Start()
    {
        textRectTransform = animationText.GetComponent<RectTransform>();
    }

    void Update()
    {
        lifetimeCount += Time.deltaTime;
        //0~1
        float t = lifetimeCount / lifetime;
        //位置の更新
        textRectTransform.localPosition = Vector3.Lerp(startPoisition, endPoisition, positionCurve.Evaluate(t));
        //アルファ値の更新
        var col = animationText.color;
        col.a = Mathf.Lerp(1, 0, alphaCurve.Evaluate(t));
        animationText.color = col;

        if (lifetimeCount > lifetime) Destroy(gameObject);
    }
}