using UnityEngine;

/// <summary>
/// プレイヤーの制御クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("移動力")]
    float movePower = 10.0f;

    [SerializeField, Tooltip("プレイヤーの番号")]
    int playerNumber = 0;
    [SerializeField, Tooltip("自分自身のRigidbody")]
    new Rigidbody rigidbody = null;

    void Update()
    {
        Move();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        float stickHorizontal = SwitchInput.GetHorizontal(playerNumber) * movePower;
        float stickVertical = SwitchInput.GetVertical(playerNumber) * movePower;

        rigidbody.AddForce(stickHorizontal, 0, stickVertical);
    }
}
