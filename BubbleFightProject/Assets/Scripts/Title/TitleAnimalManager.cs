using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimalManager : MonoBehaviour
{
    enum AnimalSpeedType
    {
        Slow,
        Normal,
        Fast
    }

    private class Lane
    {
        // 生成位置
        public Vector3 generatePos = Vector3.zero;
        // 経過時間
        public float elapsedTime = 0.0f;
        // 次の生成までの時間
        public float nextGenerateTime = 0.0f;
    }
    Lane[] LaneArray = null;

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
    [SerializeField]
    AnimalData[] animalData = null;

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
    List<Animal> animalList = new List<Animal>();

    private void Start()
    {
        int childNum = 0;
        // レーン数を数える
        foreach (Transform child in transform) { ++childNum; }
        LaneArray = new Lane[childNum];

        int laneIndex = 0;
        foreach (Transform child in transform)
        {
            Lane lane = new Lane();
            lane.generatePos = child.position;
            lane.elapsedTime = 0.0f;
            lane.nextGenerateTime = Random.Range(0.0f, 2.0f);
            LaneArray[laneIndex] = lane;
            ++laneIndex;
        }

        for (int i = 0; i < animalData.Length; ++i)
        {
            animalData[i].Init();
        }
    }

    [System.NonSerialized]
    public bool isStart = false;

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
        for (int i = 0; i < LaneArray.Length; ++i)
        {
            // 生成時間カウント
            LaneArray[i].elapsedTime += Time.deltaTime;

            if (LaneArray[i].elapsedTime > LaneArray[i].nextGenerateTime)
            {
                int animalIndex = Random.Range(0, animalData.Length);

                GameObject animalObject = Instantiate(animalData[animalIndex].animalPrefab, LaneArray[i].generatePos, Quaternion.Euler(new Vector3(0, 90, 0)));
                animalObject.transform.parent = transform;
                PlayerAnimationController playerAnimationController = animalObject.GetComponent<PlayerAnimationController>();
                playerAnimationController.AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                Animal animal = new Animal(animalObject, animalIndex);
                animalList.Add(animal);

                LaneArray[i].elapsedTime = 0.0f;
                LaneArray[i].nextGenerateTime = animalData[animalIndex].nextTime;
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
}
