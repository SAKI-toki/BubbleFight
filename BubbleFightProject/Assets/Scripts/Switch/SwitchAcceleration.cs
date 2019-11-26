using nn.hid;
using UnityEngine;

static public class SwitchAcceleration
{
    static Vector3[] Acceleration;

    static SixAxisSensorState sixAxisSensorState;
    static SixAxisSensorHandle[] gyroHandles = new SixAxisSensorHandle[1];
    static NpadStyle npadStyle;

    static public void AccelerationInit(int npadIdsLength)
    {
        Acceleration = new Vector3[npadIdsLength];
        for (int i = 0; i < npadIdsLength; ++i)
            Acceleration[i] = new Vector3();
    }

    static public void AccelerationUpdate(int index, NpadId npadId)
    {
        //未接続
        if (!SwitchManager.GetInstance().IsConnect(index)) return;

        //スタイルの取得
        npadStyle = Npad.GetStyleSet(npadId);
        SixAxisSensor.GetHandles(gyroHandles, 1, npadId, npadStyle);
        //ジャイロスタート
        SixAxisSensor.Start(gyroHandles[0]);
        SixAxisSensor.GetState(ref sixAxisSensorState, gyroHandles[0]);
        var switchAcceleration = sixAxisSensorState.acceleration;
        Acceleration[index].Set(switchAcceleration.x, switchAcceleration.y, switchAcceleration.z);
    }

    static public Vector3 GetAcceleration(int index)
    {
        //未接続
        if (!SwitchManager.GetInstance().IsConnect(index)) return Vector3.zero;
        return Acceleration[index];
    }
}