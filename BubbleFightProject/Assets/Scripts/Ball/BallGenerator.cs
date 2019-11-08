using UnityEngine;

/// <summary>
/// ボールの生成器
/// </summary>
public class BallGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("ボールPrefab")]
    GameObject ballPrefab = null;

    [SerializeField, Tooltip("ボールの生成地")]
    Transform[] ballGenerateTransforms = null;
    //ボールの数
    int ballCount = 0;
    [SerializeField, Tooltip("ボールの生成間隔")]
    float generateInterval = 10.0f;
    float generateTimeCount = 0.0f;

    //最大のボールの数
    readonly int[] MaxBallCount =
    { 1 + 2, 2 + 2, 3 + 2, 4 + 2,
      5 + 3, 6 + 3, 7 + 4, 8 + 4 };

    void Start()
    {
        PlayerJoinManager.DebugSetPlayerJoinCount(4);
        foreach (var ballController in (BallController[])GameObject.FindObjectsOfType(typeof(BallController)))
        {
            ++ballCount;
            SetDestroyEventToBallController(ballController);
        }
    }

    void Update()
    {
        generateTimeCount += Time.deltaTime;
        if (ballCount < MaxBallCount[PlayerJoinManager.GetJoinPlayerCount() - 1] && generateTimeCount >= generateInterval)
        {
            generateTimeCount = 0.0f;
            BallGenerate(ballGenerateTransforms[Random.Range(0, ballGenerateTransforms.Length)].position);
        }
    }

    /// <summary>
    /// ボールの生成
    /// </summary>
    GameObject BallGenerate(Vector3 position)
    {
        ++ballCount;
        GameObject instantiateBall = Instantiate(ballPrefab, position, Quaternion.identity);
        SetDestroyEventToBallController(instantiateBall.GetComponent<BallController>());
        return instantiateBall;
    }

    /// <summary>
    /// ボールの制御クラスのDestroyEventをセットする
    /// </summary>
    void SetDestroyEventToBallController(BallController ballController)
    {
        ballController.SetDestroyEvent(delegate { ballCount -= 1; });
    }

}