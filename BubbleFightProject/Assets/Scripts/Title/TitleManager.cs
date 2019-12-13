﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    GameObject gachaBallPrefab = null;

    [SerializeField]
    GameObject gachapon = null;

    [SerializeField]
    Transform gachaVentTransform = null;

    [SerializeField]
    Animator gachaAnimator = null;

    [SerializeField]
    Animator titleAnimator = null;

    [SerializeField]
    Material[] gachaMat = null;

    [SerializeField]
    Material skybox = null;

    [SerializeField]
    TitleAnimalManager titleAnimalManager = null;

    bool titleAnimationPlaying = true;

    private void Start()
    {
        StartCoroutine(Gacha());
    }

    private void Update()
    {
        RenderSettings.skybox = skybox;
        if (!titleAnimationPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("nextScene");
            }
        }

        if (titleAnimationPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                titleAnimator.SetTrigger("SkipTrigger");
            }
        }
    }

    // ガチャの球生成
    void GachaGenerator(float speed, int seed)
    {
        // ランダムシード値の更新
        Random.InitState(seed);
        // 飛ぶ方向を決める
        Vector3 randomPoint = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.1f, 0.5f), Random.Range(-1.0f, 1.0f));
        Vector3 dir = randomPoint - gachaVentTransform.position;
        Vector3 rotetion = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        GameObject gachaBall = Instantiate(gachaBallPrefab, gachaVentTransform.position, Quaternion.Euler(rotetion));
        gachaBall.transform.parent = gachapon.transform;
        Material mat = gachaMat[Random.Range(0, gachaMat.Length)];
        gachaBall.GetComponent<MeshRenderer>().material = mat;
        gachaBall.GetComponent<TitleGachaBallController>().Init(dir * speed);
    }

    IEnumerator Gacha()
    {
        gachaAnimator.SetTrigger("TurnTrigger");
        yield return new WaitForSeconds(1.4f);

        // ボールを出す数
        int gachaBallNum = 20;
        int count = 0;
        int seed = 1;
        float gachaIntervalTimeBase = 0.8f;
        float gachaIntervalTime = gachaIntervalTimeBase * gachaIntervalTimeBase;
        float ballSpeed = 0.04f;
        float animatorSpeed = 1.0f;

        while (gachaBallNum > count && titleAnimationPlaying)
        {
            gachaAnimator.SetTrigger("ShakeTrigger");
            // 球の生成
            GachaGenerator(ballSpeed, seed++);
            count++;
            yield return new WaitForSeconds(gachaIntervalTime);

            // 排出する時間を設定
            gachaIntervalTimeBase -= gachaIntervalTimeBase / gachaBallNum;
            gachaIntervalTime = gachaIntervalTimeBase * gachaIntervalTimeBase;
            // 球のスピードを設定
            ballSpeed += ballSpeed * 0.03f;
            // アニメーションスピードを設定
            animatorSpeed += animatorSpeed * 0.02f;
            gachaAnimator.SetFloat("Speed", animatorSpeed);
        }

        if (!titleAnimationPlaying) { yield break; }

        titleAnimator.SetTrigger("FadeTrigger");
    }

    // タイトルのアニメーション終了(AnimationEvent)
    void TitleAnimationEnd()
    {
        titleAnimationPlaying = false;
    }

    // 動物達の移動開始(AnimationEvent)
    void AnimalRun()
    {
        titleAnimalManager.isStart = true;
    }

    // skyboxを変える(AnimatinEvent)
    void SkyBoxSwitch()
    {
        RenderSettings.skybox = skybox;
    }
}