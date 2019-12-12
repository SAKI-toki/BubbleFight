using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData
{
    // 位置情報
    public List<Vector3> posList = new List<Vector3>();
    // 角度情報
    public List<Quaternion> rotList = new List<Quaternion>();

    public void Reset()
    {
        posList.Clear();
        rotList.Clear();
    }
}
