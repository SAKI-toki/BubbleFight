using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] playerPosArray = null;
    [SerializeField]
    GameObject[] kusudamaPrefab = null;
    [SerializeField]
    GameObject kamifubukiPrefab = null;

    GameObject[] kusudamaArray = null;
    Text[] rankTextArray = null;

    List<OrderData> orderList = new List<OrderData>();
    private class OrderData
    {
        public int playerId = 0;
        public int playerPoint = 0;
        public int playerRank = 0;
    }

    // デバッグ用
    // プレイヤー数
    [SerializeField]
    GameObject animalPrefab = null;
    int playerNum = 4;
    // 得点
    int[] testPoint = new int[4] { 10, 1, 4, 10 };

    void Start()
    {
        kusudamaArray = new GameObject[playerNum];
        rankTextArray = new Text[playerNum];
        SetRanking();
        Generate();
        StartCoroutine(ResultStart());
    }

    // ランクを決める
    void SetRanking()
    {
        for (int i = 0; i < playerNum; ++i)
        {
            OrderData orderData = new OrderData();
            orderData.playerId = i;
            orderData.playerPoint = testPoint[i];
            orderData.playerRank = 0;
            orderList.Add(orderData);
        }
        // 一位から決めたいため降順
        orderList.Sort((lhs, rhs)
            => rhs.playerPoint - lhs.playerPoint);
        int rank = 1;
        orderList[0].playerRank = rank;
        for (int i = 1; i < playerNum; ++i)
        {
            if (orderList[i - 1].playerPoint == orderList[i].playerPoint)
            {
                orderList[i].playerRank = rank;
            }
            else
            {
                orderList[i].playerRank = i + 1;
                rank = i + 1;
            }
        }
    }

    // 動物とくす玉を生成
    void Generate()
    {
        for (int i = 0; i < playerNum; ++i)
        {
            // 動物生成
            GameObject player = Instantiate(animalPrefab, playerPosArray[i].transform.position, playerPosArray[i].transform.rotation);
            player.transform.parent = playerPosArray[i].transform;

            // くす玉生成
            GameObject kusudama = Instantiate(kusudamaPrefab[i], playerPosArray[i].transform.position + new Vector3(0, 2, 0), playerPosArray[i].transform.rotation);
            kusudama.transform.parent = playerPosArray[i].transform;
            kusudamaArray[i] = kusudama;

            // RankTextを取得
            foreach (Transform child in kusudama.transform)
            {
                if (child.name == "Canvas")
                {
                    foreach (Transform grandchild in child)
                    {
                        if (grandchild.name == "RankText")
                        {
                            rankTextArray[i] = grandchild.GetComponent<Text>();
                            for (int j = 0; j < playerNum; ++j)
                            {
                                if (i == orderList[j].playerId)
                                {
                                    rankTextArray[i].text = orderList[j].playerRank.ToString() + "位";
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }

    IEnumerator ResultStart()
    {
        int[] counts = new int[playerNum];
        for (int i = 0; i < playerNum; ++i)
        {
            counts[orderList[i].playerRank - 1] += 1;
        }
        yield return new WaitForSeconds(0.5f);
        // くす玉を開く間隔
        const float intervalTime = 1.0f;
        int index = playerNum - 1;
        for (int i = playerNum - 1; i >= 0; --i)
        {
            for (int j = 0; j < counts[i]; ++j)
            {
                KusudamaAnimationPlay(orderList[index].playerId);
                if (i == 0)
                {
                    Instantiate(
                        kamifubukiPrefab,
                        playerPosArray[orderList[index].playerId].transform.position + new Vector3(0, 2, 0),
                        kamifubukiPrefab.transform.rotation);
                }
                --index;
            }
            if (counts[i] != 0)
            {
                yield return new WaitForSeconds(intervalTime);
            }
        }
    }
    // くす玉のアニメーションを再生
    void KusudamaAnimationPlay(int playerId)
    {
        kusudamaArray[playerId].GetComponent<Animator>().SetTrigger("OpenTrigger");
    }
}
