using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRecorder : MonoBehaviour
{
    [SerializeField]
    GameObject recordTarget = null;
    Animator animator = null;
    RecordData recordData = new RecordData();

    [SerializeField]
    float recordIntervalTime = 0.0f;
    float elapsedTime = 0.0f;

    int animationIndex = 0;

    bool isRecording;
    bool isPlayBack;

    private Animator recorderAnimator;

    void Start()
    {
        animator = recordTarget.GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("RecordCharcterにAnimatorが付いてない");
            Destroy(this);
        }

        recorderAnimator = GetComponent<Animator>();
        if (!recorderAnimator)
        {
            Debug.LogError("RecorderにAnimatorが付いてない");
        }

        isRecording = false;

        recorderAnimator.SetFloat("Speed", 1f);
    }

    // 録画を開始
    void StartRecord()
    {
        isRecording = true;

        if (isPlayBack)
        {
            StopPlayBack();
        }

        // データリセット
        recordData.Reset();

        animator.StartRecording(0);
        recorderAnimator.StartRecording(0);

        elapsedTime = 0.0f;

        //isPlayBack = false;
        Debug.Log("アニメーションの録画開始");
    }

    // 録画中
    void Recording()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > recordIntervalTime)
        {
            recordData.posList.Add(recordTarget.transform.position);
            recordData.rotList.Add(recordTarget.transform.rotation);

            elapsedTime = 0.0f;
        }
    }

    // 録画を停止
    void StopRecord()
    {
        isRecording = false;

        animator.StopRecording();
        recorderAnimator.StopRecording();

        Debug.Log("アニメーションの録画停止");
        //isPlayBack = false;
    }

    // アニメーションの再生
    void PlayBack()
    {
        //　録画してなければ何もしない
        if (animator.recorderStopTime <= 0)
        {
            return;
        }
        if (animator && recorderAnimator && !isPlayBack)
        {
            isPlayBack = true;

            animator.Rebind();
            recorderAnimator.Rebind();
            animator.StartPlayback();
            recorderAnimator.StartPlayback();
            animator.playbackTime = animator.recorderStartTime;


            recordTarget.transform.position = recordData.posList[0];
            recordTarget.transform.rotation = recordData.rotList[0];

            elapsedTime = 0.0f;
            animationIndex = 0;

            Debug.Log("アニメーションの再生");
        }
    }

    // アニメーション再生中
    void AnimationPlaying()
    {
        if (animationIndex > recordData.posList.Count - 1)
        {
            animationIndex = 0;
            elapsedTime = 0;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime > recordIntervalTime)
        {
            recordTarget.transform.position = recordData.posList[animationIndex];
            recordTarget.transform.rotation = recordData.rotList[animationIndex];

            elapsedTime = 0.0f;
        }

        ++animationIndex;


        //Debug.Log(animator.playbackTime + ":" + animator.recorderStopTime);

        float playBackTime = recorderAnimator.playbackTime + recorderAnimator.GetFloat("Speed") * Time.deltaTime;

        if (playBackTime >= recorderAnimator.recorderStopTime)
        {
            playBackTime = animator.recorderStartTime;
        }

        recorderAnimator.playbackTime = playBackTime;
        animator.playbackTime = playBackTime;
    }

    // アニメーションの停止
    void StopPlayBack()
    {
        isPlayBack = false;

        animator.StopPlayback();
        recorderAnimator.StopPlayback();
        Debug.Log("アニメーションの停止");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isRecording) { StartRecord(); }
            else { StopRecord(); }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isPlayBack) { PlayBack(); }
            else { StopPlayBack(); }
        }

        if (isRecording)
        {
            Recording();
        }

        if (isPlayBack)
        {
            AnimationPlaying();
        }
    }
}
