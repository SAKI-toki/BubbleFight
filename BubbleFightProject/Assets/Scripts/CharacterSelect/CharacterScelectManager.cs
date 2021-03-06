﻿using System.Collections.Generic;
using UnityEngine;

public class CharacterScelectManager : MonoBehaviour
{
    private class PlayerUI
    {
        public GameObject cursor;
        public Transform capusuleTransform;
        public GameObject okObj;
        public Vector3 startOkScale;
        public DecisionInfo decisionInfo;

        public PlayerUI()
        {
            cursor = null;
            capusuleTransform = null;
            okObj = null;
            startOkScale = Vector3.zero;
            decisionInfo = new DecisionInfo();
        }

        // 決定
        public void Decision(PlayerType type)
        {
            cursor.SetActive(false);
            okObj.SetActive(true);
            decisionInfo.decisionInfoType = type;
            decisionInfo.isDecision = true;
        }

        // キャンセル
        public void Cancel()
        {
            cursor.SetActive(true);
            okObj.SetActive(false);
            decisionInfo.decisionInfoType = PlayerType.None;
            decisionInfo.isDecision = false;
            okObj.transform.localScale = startOkScale;
            time = 0.0f;
        }

        float time = 0.0f;

        public void OkObjAnimation()
        {
            const float change = 0.04f;
            const int speed = 4;

            time += Time.deltaTime;
            float addNum = change * Mathf.Sin(time * speed);
            okObj.transform.localScale = startOkScale + new Vector3(addNum, addNum, 1);
        }
    }

    private class DecisionInfo
    {
        public PlayerType decisionInfoType;
        public bool isDecision;

        public DecisionInfo()
        {
            decisionInfoType = PlayerType.None;
            isDecision = false;
        }
    }

    const int playerNum = 4;
    const int animalNum = 9;

    [SerializeField]
    GameObject[] playerUIList = new GameObject[playerNum];
    PlayerUI[] playerUI = new PlayerUI[playerNum];

    [SerializeField]
    GameObject[] animalArray = new GameObject[animalNum];
    Transform[] animalMoveObj = new Transform[animalNum];
    PlayerAnimationController[] animalAnimation = new PlayerAnimationController[animalNum];
    GameObject[] message = new GameObject[animalNum];

    Vector3[] startAnimal = new Vector3[animalNum];

    Dictionary<PlayerType, int> animalIndex = new Dictionary<PlayerType, int>();

    Vector2 viewportMin = Vector2.zero;
    Vector2 viewportMax = Vector2.zero;

    Fade fade = null;

    private void Start()
    {
        fade = Fade.instance;
        fade.StartFadeIn();
        BgmManager.GetInstance().Play(BgmEnum.Select);
        viewportMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        viewportMax = Camera.main.ViewportToWorldPoint(Vector2.one);
        for (int i = 0; i < PlayerCount.MaxValue; ++i)
        {
            PlayerJoinManager.SetJoinInfo(i, false);
        }

        for (int i = 0; i < playerNum; ++i)
        {
            playerUI[i] = new PlayerUI();
            foreach (Transform child in playerUIList[i].transform)
            {
                if (child.name == "Cursor") { playerUI[i].cursor = child.gameObject; }
                else if (child.name == "Capsule") { playerUI[i].capusuleTransform = child.transform; }
                else if (child.name == "OK")
                {
                    playerUI[i].okObj = child.gameObject;
                    playerUI[i].startOkScale = child.gameObject.transform.localScale;
                    child.gameObject.SetActive(false);
                }
            }
        }

        for (int i = 0; i < animalNum; ++i)
        {
            startAnimal[i] = animalArray[i].transform.localPosition;

            foreach (Transform child in animalArray[i].transform)
            {
                if (child.tag == "Animal")
                {
                    animalMoveObj[i] = child;
                    animalAnimation[i] = child.gameObject.GetComponent<PlayerAnimationController>();
                }
                else
                {
                    message[i] = child.gameObject;
                    message[i].SetActive(false);
                }
            }
            PlayerType type = animalArray[i].GetComponent<CharaType>().type;
            animalIndex.Add(type, i);
        }
    }

    private void Update()
    {
        if (fade.IsFade) return;
        // 各プレイヤー実行
        //----------------------
        for (int i = 0; i < playerUI.Length; ++i)
        {
            // 未決定
            if (!playerUI[i].decisionInfo.isDecision)
            {
                CursorMove(i);
            }
            else
            {
                playerUI[i].OkObjAnimation();
                Cancel(i);
            }
        }
        //----------------------

        AnimalRote();

        if (SwitchInput.GetButtonDown(0, SwitchButton.Pause))
        {
            int count = 0;
            for (int i = 0; i < PlayerCount.MaxValue; ++i)
            {
                if (PlayerJoinManager.IsJoin(i))
                {
                    ++count;
                }
            }
            if (count >= PlayerCount.MinValue)
            {
                fade.StartFadeOut("GameScene");
            }
        }
    }

    /// <summary>
    /// カーソルの移動
    /// </summary>
    /// <param name="playerId"></param>
    void CursorMove(int playerId)
    {
        const float cursorSpeed = 1.5f;

        var stick = SwitchInput.GetStick(playerId) * cursorSpeed * Time.deltaTime;

        Vector3 cursorPos = playerUI[playerId].cursor.transform.position;

        playerUI[playerId].cursor.transform.position = new Vector3(
            Mathf.Clamp(cursorPos.x + stick.x, viewportMin.x, viewportMax.x),
            Mathf.Clamp(cursorPos.y + stick.y, viewportMin.y, viewportMax.y),
            cursorPos.z);

        PointerRaycast(playerId);
    }

    /// <summary>
    /// レイを飛ばして情報の取得
    /// </summary>
    /// <param name="playerId"></param>
    void PointerRaycast(int playerId)
    {
        Vector3 cursorPos = playerUI[playerId].cursor.transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(cursorPos));
        RaycastHit hit = new RaycastHit();
        const float distance = 15;

        if (Physics.Raycast(ray, out hit, distance))
        {
            GameObject obj = hit.collider.gameObject;
            PlayerType type = obj.GetComponent<CharaType>().type;

            // 決定
            if (SwitchInput.GetButtonDown(playerId, SwitchButton.Ok))
            {
                Decision(playerId, type);
            }
            else
            {
                roteNewFlg[animalIndex[type]] = true;
            }
        }
    }

    bool[] roteOldFlg = new bool[animalNum];
    bool[] roteNewFlg = new bool[animalNum];

    void AnimalRote()
    {
        for (int i = 0; i < animalNum; ++i)
        {
            if (roteNewFlg[i])
            {
                float angle = 90f * Time.deltaTime;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);

                animalMoveObj[i].transform.localRotation = q * animalMoveObj[i].transform.localRotation;

                if (!roteOldFlg[i])
                {
                    animalAnimation[i].AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                    roteOldFlg[i] = true;
                    message[i].SetActive(true);
                }
            }
            else if (roteOldFlg[i] && !roteNewFlg[i])
            {
                animalAnimation[i].AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
                roteOldFlg[i] = false;
                animalMoveObj[i].transform.localRotation = Quaternion.Euler(0, 138, 0);
                message[i].SetActive(false);
            }
            roteNewFlg[i] = false;
        }
    }

    /// <summary>
    /// 決定
    /// </summary>
    void Decision(int playerId, PlayerType type)
    {
        SeManager.GetInstance().Play(SeEnum.Decision);
        animalArray[animalIndex[type]].GetComponent<BoxCollider>().enabled = false;
        animalArray[animalIndex[type]].transform.position = new Vector3(
            playerUI[playerId].capusuleTransform.position.x - 0.01f,
            playerUI[playerId].capusuleTransform.position.y - 0.15f,
            animalArray[animalIndex[type]].transform.position.z);
        playerUI[playerId].Decision(type);
        PlayerJoinManager.SetJoinInfo(playerId, true);
        PlayerTypeManager.SetPlayerType(playerId, type);
    }

    /// <summary>
    /// 決定のキャンセル
    /// </summary>
    void Cancel(int playerId)
    {
        if (SwitchInput.GetButtonDown(playerId, SwitchButton.Cancel))
        {
            PlayerType type = playerUI[playerId].decisionInfo.decisionInfoType;
            animalArray[animalIndex[type]].GetComponent<BoxCollider>().enabled = true;
            animalArray[animalIndex[type]].transform.localPosition = startAnimal[animalIndex[type]];
            playerUI[playerId].Cancel();
            PlayerJoinManager.SetJoinInfo(playerId, false);
        }
    }
}
