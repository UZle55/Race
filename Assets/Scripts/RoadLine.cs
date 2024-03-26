using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLine : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    [SerializeField] private GameObject despawn;
    [SerializeField] private bool isForwardDirection;
    [SerializeField] private float distanceBetweenCars;
    private Queue<GameObject> carsOnLine = new Queue<GameObject>();
    private bool isReadyToSpawnNext = false;
    private float nextSpawnCoordinate;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nextSpawnCoordinate <= spawn.transform.position.z)
        {
            nextSpawnCoordinate = spawn.transform.position.z + distanceBetweenCars;
            var r = Random.Range(0, 10);
            if(r > 7)
            {
                isReadyToSpawnNext = true;
            }
        }
        CheckDespawnCar();
    }

    public void StartNewCar(GameObject car)
    {
        isReadyToSpawnNext = false;
        car.transform.position = spawn.transform.position;
        car.GetComponent<Car>().Activate(isForwardDirection);
        carsOnLine.Enqueue(car);
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

    public bool isReady()
    {
        return isReadyToSpawnNext;
    }
}
