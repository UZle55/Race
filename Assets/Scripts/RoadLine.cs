using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLine : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    [SerializeField] private GameObject despawn;
    [SerializeField] private bool isForwardDirection;
    [SerializeField] private float distanceBetweenCars;
    [SerializeField] private float distanceBetweenCoins;
    [SerializeField] private float coinRotatingSpeed;
    private Queue<GameObject> carsOnLine = new Queue<GameObject>();
    private Queue<GameObject> coinsOnLine = new Queue<GameObject>();
    private bool isReadyToSpawnNextCar = false;
    private bool isReadyToSpawnNextCoin = false;
    private float nextCarSpawnCoordinate = -9999;
    private float nextCoinSpawnCoordinate = -9999;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nextCarSpawnCoordinate <= spawn.transform.position.z)
        {
            nextCarSpawnCoordinate = spawn.transform.position.z + distanceBetweenCars;
            var r = Random.Range(0, 10);
            if(r > 7)
            {
                isReadyToSpawnNextCar = true;
            }
        }
        CheckDespawnCar();

        if (nextCoinSpawnCoordinate <= spawn.transform.position.z)
        {
            nextCoinSpawnCoordinate = spawn.transform.position.z + distanceBetweenCoins;
            var r = Random.Range(0, 10);
            if (r > 7)
            {
                isReadyToSpawnNextCoin = true;
            }
        }
        CheckDespawnCoin();
        RotateCoins();
    }

    public void StartNewCar(GameObject car)
    {
        isReadyToSpawnNextCar = false;
        car.transform.position = spawn.transform.position;
        car.GetComponent<Car>().Activate(isForwardDirection);
        carsOnLine.Enqueue(car);
    }

    public void StartNewCoin(GameObject coin)
    {
        isReadyToSpawnNextCoin = false;
        coin.transform.position = new Vector3(spawn.transform.position.x, 0.05f, spawn.transform.position.z);
        coinsOnLine.Enqueue(coin);
    }

    private void CheckDespawnCar()
    {
        if(carsOnLine.Count != 0)
        {
            if(carsOnLine.Peek().transform.position.z <= despawn.transform.position.z)
            {
                Destroy(carsOnLine.Dequeue());
            }
        }
    }

    private void CheckDespawnCoin()
    {
        if (coinsOnLine.Count != 0)
        {
            if (!coinsOnLine.Peek().activeSelf 
                    || coinsOnLine.Peek().transform.position.z <= despawn.transform.position.z)
            {
                Destroy(coinsOnLine.Dequeue());
            }
        }

    }

    private void RotateCoins()
    {
        foreach (var coin in coinsOnLine)
        {
            coin.transform.localEulerAngles += new Vector3(0, Time.deltaTime * coinRotatingSpeed, 0);
        }
    }

    public bool isReadyToSpawnCar()
    {
        return isReadyToSpawnNextCar;
    }

    public bool isReadyToSpawnCoin()
    {
        return isReadyToSpawnNextCoin;
    }
}
