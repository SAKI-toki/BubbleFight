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
    { 1 + 4, 2 + 4, 3 + 4, 4 + 5,
      5 + 5, 6 + 5, 7 + 6, 8 + 6 };

    void Start()
    {
#if UNITY_EDITOR
        PlayerJoinManager.DebugSetPlayerJoinCount(4);
#endif
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
        position.x += Random.Range(-1.0f, 1.0f);
        position.y += Random.Range(-1.0f, 1.0f);
        GameObject instantiateBall = Instantiate(ballPrefab, position, Quaternion.identity);
        instantiateBall.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * Random.Range(5, 20));
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