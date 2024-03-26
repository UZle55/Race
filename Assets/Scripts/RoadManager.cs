using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private GameObject[] roadLines;
    [SerializeField] private GameObject cars;
    [SerializeField] private GameObject goldenCoin;
    [SerializeField] private GameObject silverCoin;
    [SerializeField] private GameObject clonedCarsFirstSpawn;
    [SerializeField] private GameObject roadPart1;
    [SerializeField] private GameObject roadPart2;
    [SerializeField] private GameObject player;
    private GameObject carClone;
    private GameObject coinClone;
    private byte currentRoadPartNumber = 1;
    private float t;
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
        if (t > 1)
        {
            RepeatRoad();
            t = 0;
        }
    }

    private void CloneNextCar()
    {
        var nextCar = cars.transform.GetChild(0).gameObject;
        carClone = Instantiate(nextCar);
        carClone.transform.position = clonedCarsFirstSpawn.transform.position;
    }

    private void CloneNextCoin()
    {
        var r = Random.Range(0, 10);
        if(r > 8)
        {
            coinClone = Instantiate(goldenCoin);
        }
        else
        {
            coinClone = Instantiate(silverCoin);
        }
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
}
