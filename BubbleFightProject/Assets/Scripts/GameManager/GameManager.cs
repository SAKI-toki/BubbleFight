﻿using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームを管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームの時間を管理するクラス")]
    GameTimeManager gameTimeManager = null;

    void Start()
    {
        PointManager.Reset();
        LastHitPlayerManager.Reset();
    }

    void Update()
    {
        gameTimeManager.AddTime();
        if (!gameTimeManager.IsPlayGame()) PointManager.PointLock();
        if (SwitchInput.GetButtonDown(0, SwitchButton.Pause))
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}