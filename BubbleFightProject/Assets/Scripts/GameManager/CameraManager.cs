using UnityEngine;

static public class CameraManager
{
    static Camera[] cameras;

    static public void Reset()
    {
        cameras = new Camera[PlayerJoinManager.GetJoinPlayerCount()];
    }

    static public void SetCamera(int index, Camera camera)
    {
        cameras[PlayerJoinManager.GetNumberInPlayer(index)] = camera;
    }

    static public Camera GetCamera(int index)
    {
        return cameras[PlayerJoinManager.GetNumberInPlayer(index)];
    }
}