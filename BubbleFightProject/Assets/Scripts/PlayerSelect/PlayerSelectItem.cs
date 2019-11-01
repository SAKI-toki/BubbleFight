using UnityEngine;

/// <summary>
/// プレイヤー選択の項目
/// </summary>
public class PlayerSelectItem : SelectItemBase
{
    bool alreadyChoice = false;
    bool alreadyUpdate = false;

    public override void ItemOn()
    {
        transform.localScale = Vector3.one * 2;
    }

    public override void ItemOff()
    {
        transform.localScale = Vector3.one;
    }

    public override void ItemUpdate()
    {
        if (alreadyUpdate) return;

        alreadyUpdate = true;
    }

    void LateUpdate()
    {
        alreadyUpdate = false;
    }

    public override bool ChoiceThis()
    {
        //既に選ばれていたら選べない
        if (alreadyChoice) return false;

        alreadyChoice = true;

        return true;
    }
}