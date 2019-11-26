using UnityEngine;
using nn.hid;

public enum SwitchButton : long
{
    Ok = NpadButton.A,
    Cancel = NpadButton.B,
    Boost = NpadButton.ZR | NpadButton.ZL,
    Pause = NpadButton.Plus | NpadButton.Minus,
    //項目選択
    SelectUp = NpadButton.StickLUp,
    SelectDown = NpadButton.StickLDown,
    SelectRight = NpadButton.StickLRight,
    SelectLeft = NpadButton.StickLLeft,
}

/// <summary>
/// スイッチの入力情報
/// </summary>
static public class SwitchInput
{
    const float DeadZone = 0.2f;
    //1フレーム前のボタンの状態
    static long[] prevButtons;
    //現在のボタンの状態
    static long[] currentButtons;
    //スティックの情報
    class StickInfo
    {
        public Vector2 right, left;
    }
    //スティックの水平
    static StickInfo[] stickInfos;
    //コントローラーの状態
    static NpadState npadState = new NpadState();

    /// <summary>
    /// 入力の初期化
    /// </summary>
    /// <param name="npadIdsLength">使用するIDの配列の長さ</param>
    static public void InputInit(int npadIdsLength)
    {
        //配列の要素確保
        prevButtons = new long[npadIdsLength];
        currentButtons = new long[npadIdsLength];
        stickInfos = new StickInfo[npadIdsLength];
        for (int i = 0; i < npadIdsLength; ++i)
        {
            stickInfos[i] = new StickInfo();
        }
    }

    /// <summary>
    /// 入力の更新
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="npadId">パッドのID</param>
    static public void InputUpdate(int index, NpadId npadId)
    {
        prevButtons[index] = currentButtons[index];
        //未接続
        if (!SwitchManager.GetInstance().IsConnect(index)) return;

        //スタイルを取得
        NpadStyle npadStyle = Npad.GetStyleSet(npadId);
        //スタイルが合うかどうか
        if ((npadStyle & SwitchManager.GetInstance().GetNpadStyle()) == 0) return;
        //入力情報を取得
        Npad.GetState(ref npadState, npadId, npadStyle);
        npadState.buttons &= ~(NpadButton.StickLUp | NpadButton.StickRUp |
                                NpadButton.StickLDown | NpadButton.StickRDown |
                                NpadButton.StickLRight | NpadButton.StickRRight |
                                NpadButton.StickLLeft | NpadButton.StickRLeft);
        //スティックの更新
        //右のジョイスティック
        //デッドゾーンを超えているかどうか
        if (Mathf.Abs(npadState.analogStickR.fx) > DeadZone)
        {
            stickInfos[index].right.x = npadState.analogStickR.fx;
            npadState.buttons |= (npadState.analogStickR.fx > 0) ? NpadButton.StickRRight : NpadButton.StickRLeft;
        }
        else
        {
            stickInfos[index].right.x = 0.0f;
        }
        //デッドゾーンを超えているかどうか
        if (Mathf.Abs(npadState.analogStickR.fy) > DeadZone)
        {
            stickInfos[index].right.y = npadState.analogStickR.fy;
            npadState.buttons |= (npadState.analogStickR.fy > 0) ? NpadButton.StickRUp : NpadButton.StickRDown;
        }
        else
        {
            stickInfos[index].right.y = 0.0f;
        }
        //左のジョイスティック
        //デッドゾーンを超えているかどうか
        if (Mathf.Abs(npadState.analogStickL.fx) > DeadZone)
        {
            stickInfos[index].left.x = npadState.analogStickL.fx;
            npadState.buttons |= (npadState.analogStickL.fx > 0) ? NpadButton.StickLRight : NpadButton.StickLLeft;
        }
        else
        {
            stickInfos[index].left.x = 0.0f;
        }
        //デッドゾーンを超えているかどうか
        if (Mathf.Abs(npadState.analogStickL.fy) > DeadZone)
        {
            stickInfos[index].left.y = npadState.analogStickL.fy;
            npadState.buttons |= (npadState.analogStickL.fy > 0) ? NpadButton.StickLUp : NpadButton.StickLDown;
        }
        else
        {
            stickInfos[index].left.y = 0.0f;
        }

        currentButtons[index] = (long)npadState.buttons;
    }

    /// <summary>
    /// ボタンを今のフレームに押したか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押したならtrueを返す</returns>
    static public bool GetButtonDown(int index, NpadButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return !IsPrevButton(index, (long)button) && IsCurrentButton(index, (long)button);
    }

    /// <summary>
    /// 今のフレームにボタンを押しているか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押しているならtrueを返す</returns>
    static public bool GetButton(int index, NpadButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return IsCurrentButton(index, (long)button);
    }

    /// <summary>
    /// 今のフレームにボタンを離したか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>離したならtrueを返す</returns>
    static public bool GetButtonUp(int index, NpadButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return IsPrevButton(index, (long)button) && !IsCurrentButton(index, (long)button);
    }

    /// <summary>
    /// ボタンを今のフレームに押したか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押したならtrueを返す</returns>
    static public bool GetButtonDown(int index, SwitchButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return !IsPrevButton(index, (long)button) && IsCurrentButton(index, (long)button);
    }

    /// <summary>
    /// 今のフレームにボタンを押しているか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押しているならtrueを返す</returns>
    static public bool GetButton(int index, SwitchButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return IsCurrentButton(index, (long)button);
    }

    /// <summary>
    /// 今のフレームにボタンを離したか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>離したならtrueを返す</returns>
    static public bool GetButtonUp(int index, SwitchButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return IsPrevButton(index, (long)button) && !IsCurrentButton(index, (long)button);
    }

    /// <summary>
    /// 右スティックの入力を取得
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>右スティックの入力</returns>
    static public Vector2 GetRightStick(int index)
    {
        //未接続ならVector2.zero;
        if (!SwitchManager.GetInstance().IsConnect(index)) return Vector2.zero;
        return stickInfos[index].right;
    }

    /// <summary>
    /// 左スティックの入力を取得
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>左スティックの入力</returns>
    static public Vector2 GetLeftStick(int index)
    {
        //未接続ならVector2.zero;
        if (!SwitchManager.GetInstance().IsConnect(index)) return Vector2.zero;
        return stickInfos[index].left;
    }

    /// <summary>
    /// 1フレーム前にボタンを押していたか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押しているならtrueを返す</returns>
    static bool IsPrevButton(int index, long button)
    {
        return (prevButtons[index] & button) != 0;
    }

    /// <summary>
    /// 今のフレームにボタンを押しているか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押しているならtrueを返す</returns>
    static bool IsCurrentButton(int index, long button)
    {
        return (currentButtons[index] & button) != 0;
    }
}