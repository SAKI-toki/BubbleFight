using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ResultManager : MonoBehaviour
{
    [SerializeField, Tooltip("くす玉のオフセット")]
    Vector3 kusudamaOffset = Vector3.zero;
    [SerializeField]
    GameObject[] playerPosArray = null;
    [SerializeField]
    GameObject[] kusudamaPrefab = null;
    [SerializeField]
    GameObject kamifubukiPrefab = null;
    [SerializeField]
    Sprite[] rankSprites = null;

    GameObject[] kusudamaArray = null;
    Image[] rankTextArray = null;

    AudioSource aud = null;

    bool animEnd = false;

    FadePostprocess fade = null;

    void Start()
    {
        fade = Camera.main.GetComponent<FadePostprocess>();
        fade.StartFadeIn();
        aud = GetComponent<AudioSource>();
        PointManager.ApplyRank();
        kusudamaArray = new GameObject[PlayerCount.MaxValue];
        rankTextArray = new Image[PlayerCount.MaxValue];
        Generate();
        BgmManager.GetInstance().Stop();
        StartCoroutine(ResultStart());
    }

    void Update()
    {
        if (!animEnd || fade.IsFade) return;
        if (SwitchInput.GetButtonDown(0, SwitchButton.Ok) || Input.GetKeyDown(KeyCode.Return))
        {
            fade.StartFadeOut("TitleScene");
        }
    }

    // 動物とくす玉を生成
    void Generate()
    {
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (!PlayerJoinManager.IsJoin(i)) continue;
            // 動物生成
            GameObject player = PlayerTypeManager.GetInstance().GeneratePlayer(i, PlayerTypeManager.SceneType.Object);
            player.transform.position = playerPosArray[i].transform.position;
            player.transform.rotation = playerPosArray[i].transform.rotation;
            player.transform.localScale = playerPosArray[i].transform.localScale;
            var pos = player.transform.GetChild(0).transform.localPosition;
            pos.y = 0;
            player.transform.GetChild(0).transform.localPosition = pos;

            // くす玉生成
            GameObject kusudama = Instantiate(
                kusudamaPrefab[i],
                playerPosArray[i].transform.position + kusudamaOffset,
                playerPosArray[i].transform.rotation);
            kusudamaArray[i] = kusudama;

            // RankTextを取得
            foreach (Transform child in kusudama.transform)
            {
                if (child.name != "Canvas") continue;

                foreach (Transform grandchild in child)
                {
                    if (grandchild.name != "RankImage") continue;

                    rankTextArray[i] = grandchild.GetComponent<Image>();
                    rankTextArray[i].sprite = rankSprites[PointManager.GetRank(i) - 1];
                    break;
                }
                break;
            }
        }
    }

    IEnumerator ResultStart()
    {
        while (fade.IsFade) yield return null;
        //各順位ごとにプレイヤーの番号を格納
        List<int>[] counts = new List<int>[PlayerCount.MaxValue];
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            counts[i] = new List<int>();
        }

        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            if (!PlayerJoinManager.IsJoin(i)) continue;
            counts[PointManager.GetRank(i) - 1].Add(i);
        }

        yield return new WaitForSeconds(0.5f);

        // くす玉を開く間隔
        const float intervalTime = 1.0f;
        int index = PlayerCount.MaxValue - 1;
        for (int i = PlayerCount.MaxValue - 1; i >= 0; --i)
        {
            for (int j = 0; j < counts[i].Count; ++j)
            {
                KusudamaAnimationPlay(counts[i][j]);
                if (i == 0)
                {
                    Instantiate(
                        kamifubukiPrefab,
                        playerPosArray[counts[i][j]].transform.position + new Vector3(0, 2, 0),
                        kamifubukiPrefab.transform.rotation);
                }
                --index;
            }
            if (counts[i].Count != 0)
            {
                aud.Play();
                yield return new WaitForSeconds(intervalTime);
            }
        }

        yield return new WaitForSeconds(1.0f);
        BgmManager.GetInstance().Play(BgmEnum.Result, false);
        animEnd = true;
    }

    // くす玉のアニメーションを再生
    void KusudamaAnimationPlay(int playerId)
    {
        kusudamaArray[playerId].GetComponent<Animator>().SetTrigger("OpenTrigger");
    }
}
