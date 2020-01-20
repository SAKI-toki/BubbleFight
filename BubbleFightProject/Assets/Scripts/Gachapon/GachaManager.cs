using UnityEngine;

public class GachaManager : MonoBehaviour
{
    #region SerializeField

    [SerializeField, Header("ボールプレファブ")]
    GameObject ballPrefab = null;
    [SerializeField, Header("フェーズ情報")]
    GamePhase[] gamePhaseList = null;
    [SerializeField, Header("ガチャポンの可動域")]
    float gachaponMoveRange = 40;
    [SerializeField, Header("ガチャポンの可動スピード")]
    float gachaponMoveSpeed = 0.2f;

    #endregion SerializeField

    const int gachaNum = 4;

    int currentGamePhaseNum = 0;

    Transform[] gachaponObj = new Transform[gachaNum];
    Animator[] gachaponAnimator = new Animator[gachaNum];
    Transform[] ventPos = new Transform[gachaNum];

    private void Start()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            gachaponObj[i] = child;
            gachaponStartRotation[i] = child.eulerAngles.y;
            gachaponSetRotationYNum[i] = Random.Range(
                gachaponStartRotation[i] - (gachaponMoveRange / 2),
                gachaponStartRotation[i] + (gachaponMoveRange / 2));
            foreach (Transform grandChild in child)
            {
                if (grandChild.name == "Gachapon")
                {
                    gachaponAnimator[i] = grandChild.GetComponent<Animator>();
                    foreach (Transform greatGrandChild in grandChild)
                    {
                        if (greatGrandChild.name == "BallVentPos")
                        {
                            ventPos[i] = greatGrandChild;
                            ++i;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        for (i = 0; i < gamePhaseList.Length; ++i)
        {
            if (gamePhaseList[i].ballGeneratIntervalUpperLimitTime
                < gamePhaseList[i].ballGeneratIntervalLowerLimitTime)
            {
                Debug.LogError("gamePhaseのballGeneratIntervalが("
                    + gamePhaseList[i].ballGeneratIntervalLowerLimitTime + "～"
                    + gamePhaseList[i].ballGeneratIntervalUpperLimitTime + ")です\n" +
                    "下限が上限を超えないように変更してください");
            }
        }
        ballGeneratIntervalTime = Random.Range(
            gamePhaseList[currentGamePhaseNum].ballGeneratIntervalLowerLimitTime,
            gamePhaseList[currentGamePhaseNum].ballGeneratIntervalUpperLimitTime);
    }

    private void Update()
    {
        BallGenerator();
        Timer();
        GachaponMove();
    }

    [SerializeField]
    int currentBallNum = 0;
    float ballGeneratIntervalTime = 0.0f;
    float ballGeneratCountTime = 0.0f;

    void BallGenerator()
    {
        if (currentBallNum < gamePhaseList[currentGamePhaseNum].ballNumUpperLimit)
        {
            ballGeneratCountTime += Time.deltaTime;

            if (ballGeneratCountTime > ballGeneratIntervalTime)
            {
                BallGenerate();
            }
        }
    }

    // 玉の生成
    void BallGenerate()
    {
        int randomVent = Random.Range(0, gachaNum);
        gachaponAnimator[randomVent].SetTrigger("ShakeTrigger");
        GameObject ball = Instantiate(ballPrefab, ventPos[randomVent].position, ventPos[randomVent].rotation);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        var power = Vector3.Scale(
            ball.transform.forward * gamePhaseList[currentGamePhaseNum].ballSpeed,
            new Vector3(1 / Bumper.BouncePower, 1, 1 / Bumper.BouncePower));
        ballRigidbody.AddForce(power);

        ballGeneratIntervalTime = Random.Range(
            gamePhaseList[currentGamePhaseNum].ballGeneratIntervalLowerLimitTime,
            gamePhaseList[currentGamePhaseNum].ballGeneratIntervalUpperLimitTime);
        ++currentBallNum;
        ballGeneratCountTime = 0;
        ball.GetComponent<BallBehaviour>().SetDestroyEvent(delegate { --currentBallNum; });
    }

    float elapsedTime = 0.0f;
    void Timer()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > gamePhaseList[currentGamePhaseNum].phaseTime)
        {
            elapsedTime = 0.0f;
            // 次のフェーズに進む
            if (currentGamePhaseNum < gamePhaseList.Length - 1) { ++currentGamePhaseNum; }
        }
    }

    float[] gachaponStartRotation = new float[gachaNum];
    float[] gachaponSetRotationYNum = new float[gachaNum];
    void GachaponMove()
    {
        for (int i = 0; i < gachaNum; ++i)
        {
            if (gachaponObj[i].eulerAngles.y < gachaponSetRotationYNum[i])
            {
                gachaponObj[i].eulerAngles = new Vector3(
                    gachaponObj[i].eulerAngles.x,
                    gachaponObj[i].eulerAngles.y + gachaponMoveSpeed,
                    gachaponObj[i].eulerAngles.z);
                if (gachaponObj[i].eulerAngles.y > gachaponSetRotationYNum[i])
                {
                    gachaponSetRotationYNum[i] = Random.Range(
                        gachaponStartRotation[i] - (gachaponMoveRange / 2),
                        gachaponStartRotation[i] + (gachaponMoveRange / 2));
                }
            }
            else
            {
                gachaponObj[i].eulerAngles = new Vector3(
                    gachaponObj[i].eulerAngles.x,
                    gachaponObj[i].eulerAngles.y - gachaponMoveSpeed,
                    gachaponObj[i].eulerAngles.z);
                if (gachaponObj[i].eulerAngles.y < gachaponSetRotationYNum[i])
                {
                    gachaponSetRotationYNum[i] = Random.Range(
                        gachaponStartRotation[i] - (gachaponMoveRange / 2),
                        gachaponStartRotation[i] + (gachaponMoveRange / 2));
                }
            }
        }
    }
}
