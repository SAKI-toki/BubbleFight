using nn.hid;
using UnityEngine;

/// <summary>
/// スイッチの加速度
/// </summary>
static public class SwitchAcceleration
{
    //加速度を保持
    static Vector3[] Acceleration;
    //ステート
    static SixAxisSensorState sixAxisSensorState;
    //ハンドラ
    static SixAxisSensorHandle[] handles = new SixAxisSensorHandle[1];
    static NpadStyle npadStyle;

    /// <summary>
    /// 加速度の初期化
    /// </summary>
    static public void AccelerationInit(int npadIdsLength)
    {
        Acceleration = new Vector3[npadIdsLength];
        for (int i = 0; i < npadIdsLength; ++i)
            Acceleration[i] = new Vector3();
    }

    /// <summary>
    /// 加速度の更新
    /// </summary>
    static public void AccelerationUpdate(int index, NpadId npadId)
    {
        //未接続
        if (!SwitchManager.GetInstance().IsConnect(index)) return;

        //スタイルの取得
        npadStyle = Npad.GetStyleSet(npadId);
        SixAxisSensor.GetHandles(handles, 1, npadId, npadStyle);
        //ジャイロスタート
        SixAxisSensor.Start(handles[0]);
        SixAxisSensor.GetState(ref sixAxisSensorState, handles[0]);
        var switchAcceleration = sixAxisSensorState.acceleration;
        Acceleration[index].Set(switchAcceleration.x, switchAcceleration.y, switchAcceleration.z);
    }

    /// <summary>
    /// 加速度の取得
    /// </summary>
    static public Vector3 GetAcceleration(int index)
    {
        //未接続
        if (!SwitchManager.GetInstance().IsConnect(index)) return Vector3.zero;
        return Acceleration[index];
    }
}