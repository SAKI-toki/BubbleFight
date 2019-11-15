using UnityEngine;

/// <summary>
/// 三人称視点カメラ
/// </summary>
public class TpsCamera : MonoBehaviour
{
    [SerializeField, Tooltip("ターゲットとの距離")]
    float distance = 5.0f;
    [SerializeField, Tooltip("回転速度")]
    float rotationSpeed = 10.0f;
    [SerializeField, Tooltip("上下の最大角度"), Range(0, 89)]
    float maxAngle = 80;
    [SerializeField, Tooltip("上下の最小角度"), Range(-89, 0)]
    float minAngle = -80;
    [SerializeField, Tooltip("追尾するオブジェクト")]
    Transform targetTransform = null;
    //[SerializeField, Tooltip("カメラとターゲットの間に入ってはいけないレイヤー")]
    //LayerMask 
    [SerializeField, Tooltip("カメラ")]
    Camera tpsCamera = null;
    int index = 0;

    Vector3 rotation = Vector3.zero;

    /// <summary>
    /// カメラの初期化
    /// </summary>
    public void CameraInit(int playerIndex)
    {
        index = playerIndex;
        SetViewportRect();
    }

    /// <summary>
    /// カメラの更新
    /// </summary>
    public void CameraUpdate()
    {
        CameraRotation();
        AdjustDistance();
    }

    /// <summary>
    /// カメラの回転
    /// </summary>
    void CameraRotation()
    {
        var rightStick = SwitchInput.GetRightStick(index);
        rotation.x -= rightStick.y * Time.deltaTime * rotationSpeed;
        rotation.y += rightStick.x * Time.deltaTime * rotationSpeed;
        //縦方向の回転は制限を付ける
        rotation.x = Mathf.Clamp(rotation.x, minAngle, maxAngle);
        transform.eulerAngles = rotation;
    }

    /// <summary>
    /// ターゲットとの距離を調整する
    /// </summary>
    void AdjustDistance()
    {
        //レイを飛ばしてターゲットが隠れないようにする
        Vector3 rayDir = (tpsCamera.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, rayDir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, LayerMask.GetMask("CameraRayHitObject")))
        {
            tpsCamera.transform.localPosition = new Vector3(0, 0, -hit.distance + 1);
        }
        else
        {
            tpsCamera.transform.localPosition =
                new Vector3(0, 0, Mathf.Lerp(tpsCamera.transform.localPosition.z, -distance, Time.deltaTime * 10));
        }
    }

    /// <summary>
    /// プレイヤーのインデックスに応じて分割する
    /// </summary>
    void SetViewportRect()
    {
        var joinPlayerCount = PlayerJoinManager.GetJoinPlayerCount();
        var playNumber = PlayerJoinManager.GetNumberInPlayer(index);

        //二人の時のみ特殊化 1|2　という分割
        if (joinPlayerCount == 2)
        {
            if (playNumber == 0)
            {
                tpsCamera.rect = new Rect(0, 0, 0.5f, 1);
            }
            else
            {
                tpsCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
            }
        }
        //三人以上の時は「Z」のように二段で分割する(奇数の時の最後の枠は今のところ真っ暗)
        else
        {
            int horizonCount = Mathf.CeilToInt(joinPlayerCount / 2.0f);

            float width = 1.0f / horizonCount;

            Rect cameraRect = new Rect(0, 0, width, 0.5f);

            //上の段かどうか判定している
            if (playNumber < horizonCount)
            {
                cameraRect.y = 0.5f;
            }

            var horizonNumber = (playNumber < horizonCount) ? playNumber : playNumber - horizonCount;

            cameraRect.x = horizonNumber * width;

            tpsCamera.rect = cameraRect;
        }
    }

    /// <summary>
    /// スティックを上に倒した時の移動方向の取得
    /// </summary>
    public Vector3 GetMoveForwardDirection()
    {
        //y軸を考慮しない
        return Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
    }

    /// <summary>
    /// スティックを右に倒した時の移動方向の取得
    /// </summary>
    public Vector3 GetMoveRightDirection()
    {
        //y軸を考慮しない
        return Vector3.Scale(transform.right, new Vector3(1, 0, 1)).normalized;
    }
}
