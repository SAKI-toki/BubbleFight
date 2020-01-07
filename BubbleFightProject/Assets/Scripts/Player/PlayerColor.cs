using UnityEngine;

/// <summary>
/// プレイヤーの色
/// </summary>
static public class PlayerColor
{
    static Color[] color = new Color[4]
{
    Color.red,Color.blue,Color.yellow,Color.green
};

    /// <summary>
    /// 色の取得
    /// </summary>
    static public Color GetColor(int index)
    {
        return color[index];
    }

}