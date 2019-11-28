using nn.hid;
using UnityEngine;

/// <summary>
/// スイッチのコントローラーの色
/// </summary>
static public class SwitchColor
{
    //それぞれの色を格納
    public class RightLeftColor
    {
        public Color right = Color.white, left = Color.white;
    }

    static RightLeftColor[] colors;

    static NpadControllerColor rightNpadControllerColor, leftNpadControllerColor;

    /// <summary>
    /// 色の初期化
    /// </summary>
    static public void ColorInit(int npadIdsLength)
    {
        colors = new RightLeftColor[npadIdsLength];
        for (int i = 0; i < npadIdsLength; ++i)
            colors[i] = new RightLeftColor();
    }

    /// <summary>
    /// 色の更新
    /// </summary>
    static public void ColorUpdate(int index, NpadId npadId)
    {
        //未接続
        if (!SwitchManager.GetInstance().IsConnect(index)) return;

        Npad.GetControllerColor(ref leftNpadControllerColor, ref rightNpadControllerColor, npadId);
        var left = leftNpadControllerColor.main;
        var right = rightNpadControllerColor.main;
        colors[index].left = new Color(left.r / 255.0f, left.g / 255.0f, left.b / 255.0f, left.a / 255.0f);
        colors[index].right = new Color(right.r / 255.0f, right.g / 255.0f, right.b / 255.0f, right.a / 255.0f);
    }

    /// <summary>
    /// 色の取得
    /// </summary>
    static public RightLeftColor GetColor(int index)
    {
        //未接続
        if (!SwitchManager.GetInstance().IsConnect(index)) return new RightLeftColor();
        return colors[index];
    }
}