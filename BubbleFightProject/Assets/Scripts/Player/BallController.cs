using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BallController : MonoBehaviour
{
    Rigidbody thisRigidbody;
    Vector3 inputDir = Vector3.zero;
    Vector3 lookatDir = Vector3.zero;

    //操作するプレイヤー
    int playerIndex = 0;
    //移動力
    float movePower = 0.0f;

    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(int index, float ballMovePower)
    {
        playerIndex = index;
        movePower = ballMovePower;
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void Move()
    {
        inputDir.x = SwitchInput.GetHorizontal(playerIndex);
        inputDir.z = SwitchInput.GetVertical(playerIndex);
        //入力方向に力を加える
        thisRigidbody.AddForce(inputDir * movePower);

        if (inputDir.x == 0 && inputDir.z == 0)
        {
            //力のかかっている方向を向く
            lookatDir.x = thisRigidbody.velocity.x;
            lookatDir.z = thisRigidbody.velocity.z;
        }
        else
        {
            //入力方向を向く
            lookatDir.x = inputDir.x;
            lookatDir.z = inputDir.z;
        }
    }

    /// <summary>
    /// 向く方向を返す
    /// </summary>
    public Vector3 GetLookatDir()
    {
        return lookatDir;
    }
}