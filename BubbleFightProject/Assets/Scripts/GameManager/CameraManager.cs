using UnityEngine;

/// <summary>
/// カメラの管理クラス
/// </summary>
static public class CameraManager
{
    static Camera[] cameras;

    /// <summary>
    /// リセット
    /// </summary>
    static public void Reset()
    {
        cameras = new Camera[PlayerJoinManager.GetJoinPlayerCount()];
    }

    /// <summary>
    /// カメラのセット
    /// </summary>
    static public void SetCamera(int index, Camera camera)
    {
        cameras[PlayerJoinManager.GetNumberInPlayer(index)] = camera;
    }

    /// <summary>
    /// カメラの取得
    /// </summary>
    static public Camera GetCamera(int index)
    {
        return cameras[PlayerJoinManager.GetNumberInPlayer(index)];
    }
}