using UnityEngine;

public enum BgmEnum { Title, Game, None };

/// <summary>
/// BGMの管理クラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BgmManager : Singleton<BgmManager>
{
    //ソース
    AudioSource aud = null;

    //現在のBGM
    BgmEnum currentBgm = BgmEnum.None;

    [SerializeField]
    AudioClip title = null, game = null;

    public override void MyStart()
    {
        aud = GetComponent<AudioSource>();
    }

    public override void MyUpdate()
    { }

    /// <summary>
    /// BGMを流す
    /// </summary>
    /// <param name="bgm">どのBGMを流すか</param>
    /// <param name="is_loop">ループさせるか</param>
    /// <param name="volume">ボリューム</param>
    public void Play(BgmEnum bgm, bool is_loop = true, float volume = 1.0f)
    {
        SetVolume(volume);
        aud.loop = is_loop;
        if (currentBgm != bgm)
        {
            Stop();
            currentBgm = bgm;
        }
        switch (bgm)
        {
            case BgmEnum.Title:
                aud.clip = title;
                break;
            case BgmEnum.Game:
                aud.clip = game;
                break;
            case BgmEnum.None:
                aud.clip = null;
                break;
        }
        if (aud.clip)
            aud.Play();
    }

    /// <summary>
    /// BGMを止める
    /// </summary>
    public void Stop()
    {
        aud.Stop();
        currentBgm = BgmEnum.None;
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
