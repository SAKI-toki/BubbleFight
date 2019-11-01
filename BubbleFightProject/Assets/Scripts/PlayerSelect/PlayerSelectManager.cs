using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの選択を管理するクラス
/// </summary>
public class PlayerSelectManager : SelectItemManager
{
    //アイテムの数
    const int ItemCount = 10;

    /// <summary>
    /// プレイヤーセレクトの1プレイヤー当たりの情報
    /// </summary>
    class PlayerSelectInfo
    {
        public int index;
        public bool alreadySelect = false;
        public SelectItemBase currentSelectItem;
    }

    [SerializeField, Tooltip("アイテムの親オブジェクトのTransform")]
    Transform itemParentTransform = null;
    List<SelectItemBase> itemList = new List<SelectItemBase>();
    List<PlayerSelectInfo> playerSelectInfos = new List<PlayerSelectInfo>();

    protected override void SelectItemManagerInit()
    {
        //親オブジェクトから引っ張ってくる
        foreach (var item in itemParentTransform.GetComponentsInChildren<PlayerSelectItem>())
        {
            itemList.Add(item);
        }
        //アイテム数が既定の数と同じでなければならない
        Debug.Assert(ItemCount == itemList.Count);

        for (int i = 0; i < PlayerJoinManager.GetJoinPlayerCount(); ++i)
        {
            if (PlayerJoinManager.IsJoin(i))
            {
                var info = new PlayerSelectInfo();
                info.index = i;
                info.currentSelectItem = itemList[i];
                itemList[i].ItemOn();
                playerSelectInfos.Add(info);
            }
        }
    }

    protected override void SelectItemManagerUpdate()
    {
        //既に選んだ人数を数える変数
        int alreadyCount = 0;
        for (int i = 0; i < playerSelectInfos.Count; ++i)
        {
            //まだ選んでいなかったら
            if (!playerSelectInfos[i].alreadySelect)
            {
                CursorMove(playerSelectInfos[i].index, ref playerSelectInfos[i].currentSelectItem);
                playerSelectInfos[i].alreadySelect = ChoiceSelectItem(playerSelectInfos[i].index, playerSelectInfos[i].currentSelectItem);
            }
            else
            {
                ++alreadyCount;
            }
            //更新は毎回する
            playerSelectInfos[i].currentSelectItem.ItemUpdate();
        }

        //全員が選んだかどうか
        if (alreadyCount == PlayerJoinManager.GetJoinPlayerCount())
        {
            Debug.LogError("END");
        }
    }
}