using nn.hid;

/// <summary>
/// スイッチ関係を管理する
/// </summary>
public class SwitchManager : Singleton<SwitchManager>
{
    //使用するID
    NpadId[] npadIds = { NpadId.No1, NpadId.No2, NpadId.No3, NpadId.No4 };

    //使用するコントローラーのスタイル
    NpadStyle npadStyles = NpadStyle.JoyDual;
    //接続されているかどうか
    static bool[] isConnect;

    public override void MyStart()
    {
        //コントローラーの初期化
        Npad.Initialize();
        //サポートするタイプをセット
        Npad.SetSupportedIdType(npadIds);
        //サポートするスタイルをセット
        Npad.SetSupportedStyleSet(npadStyles);
        NpadJoy.SetHoldType(NpadJoyHoldType.Horizontal);
        //配列の要素確保
        isConnect = new bool[npadIds.Length];
        //入力の初期化
        SwitchInput.InputInit(npadIds.Length);
        //色の初期化
        SwitchColor.ColorInit(npadIds.Length);
        SwitchAcceleration.AccelerationInit(npadIds.Length);
    }

    public override void MyUpdate()
    {
        for (int i = 0; i < npadIds.Length; ++i)
        {
            //接続状態の更新
            ConnectUpdate(i);
            //入力情報の更新
            SwitchInput.InputUpdate(i, npadIds[i]);
            //色の更新
            SwitchColor.ColorUpdate(i, npadIds[i]);
            SwitchAcceleration.AccelerationUpdate(i, npadIds[i]);
        }
    }

    /// <summary>
    /// 接続状態の更新
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    void ConnectUpdate(int index)
    {
        //スタイルがNoneならfalse
        isConnect[index] = (Npad.GetStyleSet(npadIds[index]) != NpadStyle.None);
    }

    /// <summary>
    /// 接続されているか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>接続されていたらtrue</returns>
    public bool IsConnect(int index)
    {
        return isConnect[index];
    }

    /// <summary>
    /// NpadIdのゲッタ
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>NpadId</returns>
    public NpadId GetNpadId(int index)
    {
        return npadIds[index];
    }

    /// <summary>
    /// NpadStyleのゲッタ
    /// </summary>
    /// <returns>NpadStyle</returns>
    public NpadStyle GetNpadStyle()
    {
        return npadStyles;
    }
}