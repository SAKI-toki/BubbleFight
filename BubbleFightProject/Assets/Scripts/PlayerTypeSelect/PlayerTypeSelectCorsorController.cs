using UnityEngine;

/// <summary>
/// プレイヤーの種類選択のカーソルの制御
/// </summary>
public class PlayerTypeSelectCorsorController : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")]
    float moveSpeed = 3.0f;
    int playerNumber = 0;
    //既に種類を選択したかどうか
    bool alreadySelectType = false;
    [SerializeField, Tooltip("レイを飛ばす位置")]
    Transform rayTransform = null;

    PlayerTypeSelectManager.PlayerTypeEnum selectPlayerType = PlayerTypeSelectManager.PlayerTypeEnum.None;

    /// <summary>
    /// カーソルの更新
    /// </summary>
    public void CursorUpdate()
    {
        Move();
        RayUpdate();
        Select();
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        float hor = SwitchInput.GetHorizontal(playerNumber);
        float ver = SwitchInput.GetVertical(playerNumber);

        var position = transform.position;
        position.x += hor * moveSpeed * Time.deltaTime;
        position.y += ver * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    /// <summary>
    /// 種類の選択
    /// </summary>
    void Select()
    {
        if (SwitchInput.GetButtonDown(playerNumber, SwitchButton.Ok) &&
                selectPlayerType != PlayerTypeSelectManager.PlayerTypeEnum.None)
        {
            PlayerTypeSelectManager.SetPlayerType(playerNumber, selectPlayerType);
            alreadySelectType = true;
        }
    }

    /// <summary>
    /// 操作するプレイヤーの番号をセット
    /// </summary>
    public void SetPlayerNumber(int number)
    {
        playerNumber = number;
    }

    /// <summary>
    /// 既に選択したかどうか
    /// </summary>
    public bool AlreadySelectType()
    {
        return alreadySelectType;
    }

    /// <summary>
    /// レイの更新
    /// </summary>
    void RayUpdate()
    {
        Ray ray = new Ray(rayTransform.position, Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var playerTypeItem = hit.transform.GetComponent<PlayerTypeItem>();
            if (playerTypeItem)
            {
                selectPlayerType = playerTypeItem.GetPlayerType();
                return;
            }
        }
        selectPlayerType = PlayerTypeSelectManager.PlayerTypeEnum.None;
    }
}