using UnityEngine;

/// <summary>
/// プレイヤーの種類の項目
/// </summary>
public class PlayerTypeItem : MonoBehaviour
{
    [SerializeField, Tooltip("種類")]
    PlayerTypeSelectManager.PlayerTypeEnum playerType = PlayerTypeSelectManager.PlayerTypeEnum.None;

    /// <summary>
    /// プレイヤーの種類を取得
    /// </summary>
    public PlayerTypeSelectManager.PlayerTypeEnum GetPlayerType()
    {
        return playerType;
    }
}