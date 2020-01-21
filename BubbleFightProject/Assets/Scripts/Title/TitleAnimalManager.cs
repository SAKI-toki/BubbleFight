using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimalManager : MonoBehaviour
{
    #region Enum

    // 動物のスピード種類
    enum AnimalSpeedType
    {
        Slow,
        Normal,
        Fast
    }

    #endregion

    #region Private Class

    private class Lane
    {
        // 生成位置
        public Vector3 generatePos = Vector3.zero;
        // 経過時間
        public float elapsedTime = 0.0f;
        // 次の生成までの時間
        public float nextGenerateTime = 0.0f;
    }

    [System.Serializable]
    private class AnimalData
    {
        [SerializeField]
        public GameObject animalPrefab = null;
        [SerializeField]
        AnimalSpeedType animalSpeedType = 0;
        public float speed { get; private set; }
        public float nextTime { get; private set; }

        public void Init()
        {
            switch (this.animalSpeedType)
            {
                case AnimalSpeedType.Slow:
                    this.speed = 0.025f;
                    this.nextTime = 5.0f;
                    break;
                case AnimalSpeedType.Normal:
                    this.speed = 0.03f;
                    this.nextTime = 4.0f;
                    break;
                case AnimalSpeedType.Fast:
                    this.speed = 0.035f;
                    this.nextTime = 3.0f;
                    break;
            }
        }
    }

    private class Animal
    {
        public GameObject animalObject { get; private set; }
        public int animalId { get; private set; }

        public Animal(GameObject obj, int id)
        {
            animalObject = obj;
            animalId = id;
        }
    }

    #endregion Private Class

    // レーン情報
    Lane[] laneArray = null;

    // 動物の情報
    [SerializeField]
    AnimalData[] animalData = null;

    // 生成した動物
    List<Animal> animalList = new List<Animal>();

    // 動物の生成順番
    int[] animalGenerateOrder = null;
    int animalGeneratCurrentIndex = 0;

    [System.NonSerialized]
    public bool isStart = false;

    private void Start()
    {
        int childNum = 0;
        // レーン数を数える
        foreach (Transform child in transform) { ++childNum; }
        laneArray = new Lane[childNum];

        int laneIndex = 0;
        foreach (Transform child in transform)
        {
            Lane lane = new Lane();
            lane.generatePos = child.position;
            lane.elapsedTime = 0.0f;
            lane.nextGenerateTime = Random.Range(0.0f, 2.0f);
            laneArray[laneIndex] = lane;
            ++laneIndex;
        }

        for (int i = 0; i < animalData.Length; ++i)
        {
            animalData[i].Init();
        }
        animalGenerateOrder = new int[animalData.Length];
        AnimalGeneratRandom();
    }


    private void Update()
    {
        if (isStart)
        {
            Generate();
            Move();
            AnimalDestroy();
        }
    }

    /// <summary>
    /// 動物の生成
    /// </summary>
    void Generate()
    {
        for (int i = 0; i < laneArray.Length; ++i)
        {
            // 生成時間カウント
            laneArray[i].elapsedTime += Time.deltaTime;

            if (laneArray[i].elapsedTime > laneArray[i].nextGenerateTime)
            {
                int animalIndex = animalGenerateOrder[animalGeneratCurrentIndex];

                GameObject animalObject = Instantiate(animalData[animalIndex].animalPrefab, laneArray[i].generatePos, Quaternion.Euler(new Vector3(0, 90, 0)));
                animalObject.transform.parent = transform;
                PlayerAnimationController playerAnimationController = animalObject.GetComponent<PlayerAnimationController>();
                playerAnimationController.AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                Animal animal = new Animal(animalObject, animalIndex);
                animalList.Add(animal);

                laneArray[i].elapsedTime = 0.0f;
                laneArray[i].nextGenerateTime = animalData[animalIndex].nextTime;
                if (animalGeneratCurrentIndex < animalGenerateOrder.Length - 1)
                {
                    ++animalGeneratCurrentIndex;
                }
                else
                {
                    AnimalGeneratRandom();
                }
            }
        }
    }

    /// <summary>
    /// 動物移動
    /// </summary>
    void Move()
    {
        for (int i = 0; i < animalList.Count; ++i)
        {
            animalList[i].animalObject.transform.position = new Vector3(
                animalList[i].animalObject.transform.position.x + animalData[animalList[i].animalId].speed,
                animalList[i].animalObject.transform.position.y,
                animalList[i].animalObject.transform.position.z);
        }
    }

    /// <summary>
    /// 一定距離に行った動物を消す
    /// </summary>
    void AnimalDestroy()
    {
        for (int i = 0; i < animalList.Count; ++i)
        {
            if (animalList[i].animalObject.transform.position.x > 5)
            {
                Destroy(animalList[i].animalObject);
                animalList.RemoveAt(i);
            }
        }
    }

    // 生成する動物の種類に規則性を持たせる
    void AnimalGeneratRandom()
    {
        animalGeneratCurrentIndex = 0;

        for (int i = 0; i < animalGenerateOrder.Length; ++i)
        {
            animalGenerateOrder[i] = i;
        }

        // ランダム
        for (int i = 0; i < animalGenerateOrder.Length; ++i)
        {
            int randomIndex = Random.Range(0, animalGenerateOrder.Length);
            int temp = 0;

            temp = animalGenerateOrder[i];
            animalGenerateOrder[i] = animalGenerateOrder[randomIndex];
            animalGenerateOrder[randomIndex] = temp;
        }
    }
}
