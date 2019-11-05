using UnityEngine;

/// <summary>
/// セレクトする項目の基底クラス
/// </summary>
public abstract class SelectItemBase : MonoBehaviour
{
    [SerializeField, Tooltip("上を押したときに移動する項目")]
    public SelectItemBase upSelectItem = null;
    [SerializeField, Tooltip("下を押したときに移動する項目")]
    public SelectItemBase downSelectItem = null;
    [SerializeField, Tooltip("右を押したときに移動する項目")]
    public SelectItemBase rightSelectItem = null;
    [SerializeField, Tooltip("左を押したときに移動する項目")]
    public SelectItemBase leftSelectItem = null;
    [SerializeField, Tooltip("右上を押したときに移動する項目")]
    public SelectItemBase rightupSelectItem = null;
    [SerializeField, Tooltip("右下を押したときに移動する項目")]
    public SelectItemBase rightdownSelectItem = null;
    [SerializeField, Tooltip("左上を押したときに移動する項目")]
    public SelectItemBase leftupSelectItem = null;
    [SerializeField, Tooltip("左下を押したときに移動する項目")]
    public SelectItemBase leftdownSelectItem = null;

    virtual public void ItemOn() { }
    virtual public void ItemUpdate() { }
    virtual public void ItemOff() { }
    virtual public bool ChoiceThis() { return true; }

    void Start() { }
    void Update() { }
}

/// <summary>
/// セレクトする項目の管理クラス
/// </summary>
public abstract class SelectItemManager : MonoBehaviour
{
    void Start()
    {
        SelectItemManagerInit();
    }

    void Update()
    {
        SelectItemManagerUpdate();
    }

    /// <summary>
    /// カーソルの移動
    /// </summary>
    protected void CursorMove(int operationIndex, ref SelectItemBase currentSelectItem)
    {
        bool up = SwitchInput.GetButtonDown(operationIndex, SwitchButton.StickUp);
        bool down = SwitchInput.GetButtonDown(operationIndex, SwitchButton.StickDown);
        bool right = SwitchInput.GetButtonDown(operationIndex, SwitchButton.StickRight);
        bool left = SwitchInput.GetButtonDown(operationIndex, SwitchButton.StickLeft);

        SelectItemBase nextSelectItem = currentSelectItem;

        //入力したキーに応じて遷移する
        if (right && up && currentSelectItem.rightupSelectItem != null) nextSelectItem = currentSelectItem.rightupSelectItem;
        else if (left && up && currentSelectItem.leftupSelectItem != null) nextSelectItem = currentSelectItem.leftupSelectItem;
        else if (right && down && currentSelectItem.rightdownSelectItem != null) nextSelectItem = currentSelectItem.rightdownSelectItem;
        else if (left && down && currentSelectItem.leftdownSelectItem != null) nextSelectItem = currentSelectItem.leftdownSelectItem;
        else if (up && currentSelectItem.upSelectItem != null) nextSelectItem = currentSelectItem.upSelectItem;
        else if (down && currentSelectItem.downSelectItem != null) nextSelectItem = currentSelectItem.downSelectItem;
        else if (right && currentSelectItem.rightSelectItem != null) nextSelectItem = currentSelectItem.rightSelectItem;
        else if (left && currentSelectItem.leftSelectItem != null) nextSelectItem = currentSelectItem.leftSelectItem;

        ChangeSelectItem(nextSelectItem, ref currentSelectItem);
    }

    /// <summary>
    /// 選んでいる項目の変更
    /// </summary>
    void ChangeSelectItem(SelectItemBase nextSelectItem, ref SelectItemBase currentSelectItem)
    {
        if (nextSelectItem != null && nextSelectItem != currentSelectItem)
        {
            currentSelectItem.ItemOff();
            currentSelectItem = nextSelectItem;
            currentSelectItem.ItemOn();
        }
    }

    /// <summary>
    /// アイテムを選ぶ
    /// </summary>
    protected bool ChoiceSelectItem(int operationIndex, SelectItemBase currentSelectItem)
    {
        if (SwitchInput.GetButtonDown(operationIndex, SwitchButton.Ok))
        {
            return currentSelectItem.ChoiceThis();
        }
        return false;
    }

    protected virtual void SelectItemManagerInit() { }
    protected virtual void SelectItemManagerUpdate() { }
}