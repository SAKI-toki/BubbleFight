using UnityEngine;

/// <summary>
/// プレイヤーの項目の種類
/// </summary>
public class PlayerItemType : MonoBehaviour
{
    [SerializeField, Tooltip("動物の種類")]
    PlayerType playerType = PlayerType.None;

    [SerializeField]
    bool alreadySelect = false;

    /// <summary>
    /// 種類の取得
    /// </summary>
    public PlayerType GetPlayerType()
    {
        return playerType;
    }

    /// <summary>
    /// 既に選択されたかどうか
    /// </summary>
    public bool GetAlreadySelect()
    {
        return alreadySelect;
    }

    /// <summary>
    /// 選択したかどうかセット
    /// </summary>
    public void SetAlreadySelect(bool isSelect)
    {
        alreadySelect = isSelect;
    }
}
