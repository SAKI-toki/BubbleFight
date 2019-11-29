using UnityEngine;

/// <summary>
/// カーソルの制御
/// </summary>
public class CursorController : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")]
    float moveSpeed = 10.0f;
    [SerializeField, Tooltip("移動できる範囲(絶対値)")]
    Vector2 moveRange = Vector2.zero;
    [SerializeField, Tooltip("項目のレイヤー")]
    LayerMask itemLayer = default(LayerMask);
    int playerIndex = 0;
    [SerializeField]
    PlayerItemType selectType = null;

    public void CursorUpdate()
    {
        var position = transform.position;
        var stick = SwitchInput.GetLeftStick(playerIndex);
        position.x += stick.x * moveSpeed * Time.deltaTime;
        position.y += stick.y * moveSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, -moveRange.x, moveRange.x);
        position.y = Mathf.Clamp(position.y, -moveRange.y, moveRange.y);
        transform.position = position;

        //選択
        if (!AlreadySelectType() && SwitchInput.GetButtonDown(playerIndex, SwitchButton.Ok))
        {
            Ray ray = new Ray(transform.position, Vector3.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, itemLayer))
            {
                var playerItemType = hit.transform.gameObject.GetComponent<PlayerItemType>();
                if (playerItemType != null && !playerItemType.GetAlreadySelect())
                {
                    selectType = playerItemType;
                    selectType.SetAlreadySelect(true);
                    PlayerTypeManager.SetPlayerType(playerIndex, selectType.GetPlayerType());
                    transform.localScale = Vector3.one * 2;
                }
            }
        }
        //選択キャンセル
        else if (AlreadySelectType() && SwitchInput.GetButtonDown(playerIndex, SwitchButton.Cancel))
        {
            selectType.SetAlreadySelect(false);
            selectType = null;
            transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// プレイヤーの番号のセット
    /// </summary>
    public void SetPlayerNumber(int number)
    {
        playerIndex = number;
    }

    /// <summary>
    /// 既に選んだかどうか
    /// </summary>
    public bool AlreadySelectType()
    {
        return selectType != null;
    }
}