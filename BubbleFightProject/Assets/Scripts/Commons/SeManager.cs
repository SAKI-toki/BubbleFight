using UnityEngine;

public enum SeEnum { Decision, None };

/// <summary>
/// SEの管理クラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SeManager : Singleton<SeManager>
{
    // ソース
    AudioSource aud = null;

    // 現在のSE
    SeEnum currentSe = SeEnum.None;

    [SerializeField]
    AudioClip decision = null;

    public override void MyStart()
    {
        aud = GetComponent<AudioSource>();
    }

    public override void MyUpdate()
    { }

    /// <summary>
    /// BGMを流す
    /// </summary>
    /// <param name="se">どのSEを流すか</param>
    /// <param name="is_loop">ループさせるか</param>
    /// <param name="volume">ボリューム</param>
    public void Play(SeEnum se, bool is_loop = false, float volume = 1.0f)
    {
        SetVolume(volume);
        aud.loop = is_loop;
        if (currentSe != se)
        {
            Stop();
            currentSe = se;
        }
        switch (se)
        {
            case SeEnum.Decision:
                aud.clip = decision;
                break;
            case SeEnum.None:
                aud.clip = null;
                break;
        }
        if (aud.clip)
            aud.Play();
    }

    /// <summary>
    /// SEを止める
    /// </summary>
    public void Stop()
    {
        aud.Stop();
        currentSe = SeEnum.None;
    }

    /// <summary>
    /// ボリュームのセット
    /// </summary>
    /// <param name="volume">セットするボリューム</param>
    public void SetVolume(float volume)
    {
        aud.volume = volume;
    }

    /// <summary>
    /// ボリュームの取得
    /// </summary>
    /// <returns>ボリューム</returns>
    public float GetVolume()
    {
        return aud.volume;
    }
}
