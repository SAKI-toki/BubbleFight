using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Transform cameraTransform = null;
    //初期位置
    Vector3 initPosition = Vector3.zero;
    static float timeCount = 0.0f;
    Vector3 incrementPosition = Vector3.zero;
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        initPosition = cameraTransform.position;
        timeCount = 0.0f;
    }
    void Update()
    {
        //揺れる
        if (timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            incrementPosition.x = Mathf.Sin(timeCount * 1000) / 2;
            incrementPosition.y = Mathf.Cos(timeCount * 1000) / 2;
            incrementPosition.z = Mathf.Sin(timeCount * 500) / 2;
            cameraTransform.position = initPosition + incrementPosition;
        }
        else
        {
            cameraTransform.position = initPosition;
        }
    }

    /// <summary>
    /// 揺らす
    /// </summary>
    static public void Shake(float shakeTime)
    {
        timeCount = Mathf.Max(shakeTime, timeCount);
    }
}
