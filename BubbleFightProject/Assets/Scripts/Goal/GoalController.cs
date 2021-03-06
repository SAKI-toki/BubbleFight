﻿using UnityEngine;

/// <summary>
/// ゴールの制御クラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class GoalController : MonoBehaviour
{
    [SerializeField, Tooltip("ゴールの番号")]
    int goalNumber = 0;
    Transform childTransform = null;
    bool zeroPointFlag = false;
    [SerializeField]
    GameObject[] destroyZeroPointObjects = null;
    AudioSource goalAudio = null;

    void Start()
    {
        goalAudio = GetComponent<AudioSource>();
        childTransform = transform.GetChild(0);
        var mat = childTransform.GetComponent<MeshRenderer>().material;
        mat.color = PlayerColor.GetColor(goalNumber);
        childTransform.GetComponent<MeshRenderer>().material = mat;
        if (!PlayerJoinManager.IsJoin(goalNumber))
        {
            Zeropoint();
        }
    }

    void Update()
    {
        if (!zeroPointFlag && PointManager.GetPoint(goalNumber) <= 0)
        {
            Zeropoint();
        }
    }

    /// <summary>
    /// ゴールの番号を取得
    /// </summary>
    public int GetGoalNumber()
    {
        return goalNumber;
    }

    void Zeropoint()
    {
        zeroPointFlag = true;
        var scale = childTransform.localScale;
        scale.y = 4;
        childTransform.localScale = scale;
        foreach (var obj in destroyZeroPointObjects)
        {
            if (obj) Destroy(obj);
        }
    }

    /// <summary>
    /// ゴールに入った時の音を鳴らす
    /// </summary>
    public void goalAudioPlay()
    {
        goalAudio.Play();
    }
}
