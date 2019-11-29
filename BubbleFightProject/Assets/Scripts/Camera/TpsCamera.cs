using UnityEngine;

/// <summary>
/// 三人称視点カメラ
/// </summary>
public class TpsCamera : MonoBehaviour
{
    [SerializeField, Tooltip("ターゲットとのデフォルトの距離")]
    float defaultTargetDistance = 5.0f;
    [SerializeField, Tooltip("ターゲットとの最大のズレ")]
    float maxTargetSlip = 3.0f;
    [SerializeField, Tooltip("左右の回転速度")]
    float horizontalRotationSpeed = 10.0f;
    [SerializeField, Tooltip("上下の回転速度")]
    float verticalRotationSpeed = 10.0f;
    [SerializeField, Tooltip("位置の補間速度")]
    float positionLerpSpeed = 10.0f;
    [SerializeField, Tooltip("カメラの回転の曲線")]
    AnimationCurve rotationCurve = new AnimationCurve();
    [SerializeField, Tooltip("上下の最大角度"), Range(0, 89)]
    float maxAngle = 80;
    [SerializeField, Tooltip("上下の最小角度"), Range(-89, 0)]
    float minAngle = -80;
    [SerializeField, Tooltip("カメラとターゲットの間に入ってはいけないレイヤー")]
    LayerMask rayHitLayerMask = default(LayerMask);
    [SerializeField, Tooltip("カメラ")]
    Camera tpsCamera = null;
    [SerializeField, Tooltip("親オブジェクト")]
    Transform parentTransform = null;
    [SerializeField, Tooltip("子オブジェクト")]
    Transform childTransform = null;
    [SerializeField, Tooltip("カメラオブジェクト")]
    Transform cameraTransform = null;

    int index = 0;
    //追尾するターゲット
    Transform targetTransform = null;
    Vector3 rotation = Vector3.zero;
    //揺れる時間を測る
    [SerializeField]
    float shakeTimeCount = 0.0f;
    [SerializeField]
    //揺れる力
    float shakePower = 0.0f;

    bool isFirst = true;

    /// <summary>
    /// カメラの初期化
    /// </summary>
    public void CameraInit(int playerIndex, Transform target)
    {
        targetTransform = target;
        index = playerIndex;
        //parentTransform.position = targetTransform.position;
        //rotation = target.transform.eulerAngles;
        SetViewportRect();
        CameraManager.SetCamera(playerIndex, tpsCamera);
    }

    /// <summary>
    /// カメラの更新
    /// </summary>
    public void CameraUpdate()
    {
        if (isFirst)
        {
            isFirst = false;
            FirstInterpolation();
        }
        CameraRotation();
        Interpolation();
    }

    /// <summary>
    /// カメラの回転
    /// </summary>
    void CameraRotation()
    {
        var rightStick = SwitchInput.GetRightStick(index);
        //曲線を適応する値
        float evaluateValue = rotationCurve.Evaluate(rightStick.magnitude);
        rightStick *= Mathf.Clamp(evaluateValue, 0, 1);
        rotation.x -= rightStick.y * Time.deltaTime * verticalRotationSpeed;
        rotation.y += rightStick.x * Time.deltaTime * horizontalRotationSpeed;
        //縦方向の回転は制限を付ける
        rotation.x = Mathf.Clamp(rotation.x, minAngle, maxAngle);
        parentTransform.eulerAngles = rotation;
    }

    /// <summary>
    /// 各値の補間
    /// </summary>
    void Interpolation()
    {
        DistanceInterpolation();
        PositionInterpolation();
        ShakeInterpolation();
    }

    /// <summary>
    /// ターゲットとの距離を補間する
    /// </summary>
    void DistanceInterpolation()
    {
        //レイを飛ばしてターゲットが隠れないようにする
        Ray ray = new Ray(parentTransform.position, (childTransform.position - parentTransform.position).normalized);
        RaycastHit hit;
        //何かにぶつかったらそれが隠れないように近くに寄る
        if (Physics.Raycast(ray, out hit, defaultTargetDistance, rayHitLayerMask))
        {
            childTransform.localPosition = new Vector3(0, 0, -hit.distance + 1);
        }
        //ぶつからなかったらデフォルトの距離まで離す
        else
        {
            childTransform.localPosition =
                new Vector3(0, 0, Mathf.Lerp(childTransform.localPosition.z, -defaultTargetDistance, Time.deltaTime * 10));
        }
    }

    /// <summary>
    /// 位置を補間する
    /// </summary>
    void PositionInterpolation()
    {
        parentTransform.position = Vector3.Lerp(parentTransform.position, targetTransform.position, positionLerpSpeed * Time.deltaTime);
        float slip = Vector3.Distance(parentTransform.position, targetTransform.position);
        //離れすぎている場合近くに寄せる
        if (slip > maxTargetSlip)
        {
            parentTransform.position = Vector3.Lerp(parentTransform.position, targetTransform.position,
                (slip - maxTargetSlip) / maxTargetSlip);
        }
    }

    /// <summary>
    /// カメラの揺れを補間する
    /// </summary>
    void ShakeInterpolation()
    {
        //揺れる
        if (shakeTimeCount > 0.0f)
        {
            shakeTimeCount -= Time.deltaTime;
            cameraTransform.localPosition =
                new Vector3(Random.Range(-shakePower, shakePower),
                    Random.Range(-shakePower, shakePower),
                    Random.Range(-shakePower, shakePower));
        }
        //元の位置に戻す({0,0,0})
        else
        {
            cameraTransform.localPosition = Vector3.zero;
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
            //横のプレイヤーの数
            int horizonCount = Mathf.CeilToInt(joinPlayerCount / 2.0f);
            //一人あたりの画面占有率(幅)
            float width = 1.0f / horizonCount;
            //セットする矩形
            Rect cameraRect = new Rect(0, 0, width, 0.5f);
            //上の段かどうか判定している
            if (playNumber < horizonCount)
            {
                cameraRect.y = 0.5f;
            }
            //右から何番目か
            var horizonNumber = (playNumber < horizonCount) ? playNumber : playNumber - horizonCount;
            cameraRect.x = horizonNumber * width;
            tpsCamera.rect = cameraRect;
        }
    }

    /// <summary>
    /// 最初の補間
    /// </summary>
    void FirstInterpolation()
    {
        parentTransform.position = targetTransform.position;
        rotation = targetTransform.eulerAngles;
    }

    /// <summary>
    /// スティックを上に倒した時の移動方向の取得
    /// </summary>
    public Vector3 GetMoveForwardDirection()
    {
        //y軸を考慮しない
        return Vector3.Scale(parentTransform.forward, new Vector3(1, 0, 1)).normalized;
    }

    /// <summary>
    /// スティックを右に倒した時の移動方向の取得
    /// </summary>
    public Vector3 GetMoveRightDirection()
    {
        //y軸を考慮しない
        return Vector3.Scale(parentTransform.right, new Vector3(1, 0, 1)).normalized;
    }

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    public void CameraShake(float time, float power)
    {
        shakeTimeCount = time;
        shakePower = power;
    }

    /// <summary>
    /// 回転を直接セット
    /// </summary>
    public void SetRotation(Vector3 rot)
    {
        rotation = rot;
    }
}
