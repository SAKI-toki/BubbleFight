using UnityEngine;

public class GachaManager : MonoBehaviour
{
    const int gachaNum = 4;
    [SerializeField]
    GamePhase[] gamePhaseList;
    int gamePhaseNum = 0;

    Transform[] gachaponObj = new Transform[gachaNum];
    Animator[] gachaponAnimator = new Animator[gachaNum];
    Transform[] ventPos = new Transform[gachaNum];
    float[] gachaponStartRotation = new float[gachaNum];
    float[] gachaponSetRotationYNum = new float[gachaNum];

    [SerializeField]
    GameObject ballPrefab = null;

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
            gamePhaseList[gamePhaseNum].ballGeneratIntervalLowerLimitTime,
            gamePhaseList[gamePhaseNum].ballGeneratIntervalUpperLimitTime);
    }

    private void Update()
    {
        BallGenerator();
        Timer();
        GachaponMove();
    }

    int ballCurrentNum = 0;
    float ballGeneratIntervalTime = 0.0f;
    float ballGeneratCountTime = 0.0f;

    void BallGenerator()
    {
        if (ballCurrentNum < gamePhaseList[gamePhaseNum].ballNumUpperLimit)
        {
            ballGeneratCountTime += Time.deltaTime;

            if (ballGeneratCountTime > ballGeneratIntervalTime)
            {
                BallGenerat();
            }
        }
    }

    // 玉の生成
    void BallGenerat()
    {
        int randomVent = Random.Range(0, gachaNum);
        gachaponAnimator[randomVent].SetTrigger("ShakeTrigger");
        GameObject ball = Instantiate(ballPrefab, ventPos[randomVent].position, ventPos[randomVent].rotation);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.AddForce(ball.transform.forward * gamePhaseList[gamePhaseNum].ballSpeed);

        ballGeneratIntervalTime = Random.Range(
            gamePhaseList[gamePhaseNum].ballGeneratIntervalLowerLimitTime,
            gamePhaseList[gamePhaseNum].ballGeneratIntervalUpperLimitTime);
        ++ballCurrentNum;
        ballGeneratCountTime = 0;
    }

    float elapsedTime = 0.0f;
    void Timer()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > gamePhaseList[gamePhaseNum].phaseTime)
        {
            elapsedTime = 0.0f;
            // 次のフェーズに進む
            if (gamePhaseNum < gamePhaseList.Length - 1) { ++gamePhaseNum; }
        }
    }

    float gachaponMoveRange = 40;
    float gachaponMoveSpeed = 0.2f;
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
