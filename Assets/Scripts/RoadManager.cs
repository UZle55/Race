using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private GameObject[] roadLines;
    [SerializeField] private GameObject cars;
    [SerializeField] private GameObject goldenCoin;
    [SerializeField] private GameObject clonedCarsFirstSpawn;
    [SerializeField] private GameObject roadPart1;
    [SerializeField] private GameObject roadPart2;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvas;
    private GameObject carClone;
    private GameObject coinClone;
    private byte currentRoadPartNumber = 1;
    private float t;
    public static bool IsTutorial { get; private set; }
    public static bool IsEndlessMode { get; private set; }
    public static int[] TimeToSet { get; private set; } = new int[] { 0, 0, 0 };
    public static int CoinsCountAim { get; private set; }
    public static int CurrentLevelNumber { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        CloneNextCar();
        CloneNextCoin();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (!IsTutorial)
        {
            foreach (var roadLine in roadLines)
            {
                if (roadLine.GetComponent<RoadLine>().isReadyToSpawnCar())
                {
                    roadLine.GetComponent<RoadLine>().StartNewCar(carClone);
                    CloneNextCar();
                }
                if (roadLine.GetComponent<RoadLine>().isReadyToSpawnCoin())
                {
                    roadLine.GetComponent<RoadLine>().StartNewCoin(coinClone);
                    CloneNextCoin();
                }
            }
        }
        if (t > 1)
        {
            RepeatRoad();
            t = 0;
        }
    }

    private void CloneNextCar()
    {
        int r = Random.Range(0, cars.transform.childCount);
        var nextCar = cars.transform.GetChild(r).gameObject;
        carClone = Instantiate(nextCar);
        carClone.transform.position = clonedCarsFirstSpawn.transform.position;
    }

    private void CloneNextCoin()
    {
        coinClone = Instantiate(goldenCoin);
        coinClone.transform.position = goldenCoin.transform.position;
    }

    private void RepeatRoad()
    {
        if(currentRoadPartNumber == 1)
        {
            if (Mathf.Abs(player.transform.position.z - roadPart1.transform.position.z) < 5)
            {
                roadPart2.transform.position = new Vector3(0, roadPart1.transform.position.y - 0.005f, roadPart1.transform.position.z + 50);
                currentRoadPartNumber = 2;
            }
        }
        if(currentRoadPartNumber == 2)
        {
            if (Mathf.Abs(player.transform.position.z - roadPart2.transform.position.z) < 5)
            {
                roadPart1.transform.position = new Vector3(0, roadPart2.transform.position.y - 0.005f, roadPart2.transform.position.z + 50);
                currentRoadPartNumber = 1;
            }
        }
    }

    public static void SetSettings(bool isTutorial, int[] timeToSet, int coinsCountAim, bool isEndless, int levelNumber)
    {
        IsTutorial = isTutorial;
        TimeToSet = timeToSet;
        CoinsCountAim = coinsCountAim;
        IsEndlessMode = isEndless;
        CurrentLevelNumber = levelNumber;
    }

    public static void EndTutorial()
    {
        IsTutorial = false;
    }
}
