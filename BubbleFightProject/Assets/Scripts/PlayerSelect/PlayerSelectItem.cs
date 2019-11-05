using UnityEngine;

/// <summary>
/// プレイヤー選択の項目
/// </summary>
public class PlayerSelectItem : SelectItemBase
{
    bool alreadyChoice = false;
    bool alreadyUpdate = false;

    /// <summary>
    /// この項目を選ばれていた
    /// </summary>
    public override void ItemOn()
    {
        transform.localScale = Vector3.one * 2;
    }

    /// <summary>
    /// この項目を選ばれなくなった
    /// </summary>
    public override void ItemOff()
    {
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// この項目の更新
    /// </summary>
    public override void ItemUpdate()
    {
        if (alreadyUpdate) return;

        alreadyUpdate = true;
    }

    void LateUpdate()
    {
        alreadyUpdate = false;
    }

    /// <summary>
    /// この項目の選択された
    /// </summary>
    public override bool ChoiceThis()
    {
        //既に選ばれていたら選べない
        if (alreadyChoice) return false;

        alreadyChoice = true;

        return true;
    }
}