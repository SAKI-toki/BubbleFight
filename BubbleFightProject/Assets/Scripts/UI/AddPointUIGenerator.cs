using UnityEngine;

/// <summary>
/// ポイントの追加のUIの生成クラス
/// </summary>
public class AddPointUIGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("+のUIリスト")]
    GameObject[] PlusUIList = null;
    [SerializeField, Tooltip("-のUIリスト")]
    GameObject[] MinusUIList = null;

    /// <summary>
    /// 外部から呼び出す生成関数
    /// </summary>
    public void AddPoint(int index, int point)
    {
        if (point > 0)
        {
            PlusPoint(point);
        }
        else
        {
            MinusPoint(point);
        }
    }

    /// <summary>
    /// プラスの呼び出し
    /// </summary>
    void PlusPoint(int point)
    {
        //未実装
    }

    /// <summary>
    /// マイナスの呼び出し
    /// </summary>
    void MinusPoint(int point)
    {
        //未実装
    }
}