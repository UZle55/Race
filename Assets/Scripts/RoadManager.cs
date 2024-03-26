using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private GameObject[] roadLines;
    [SerializeField] private GameObject cars;
    [SerializeField] private GameObject clonedCarsFirstSpawn;
    private GameObject carClone;
    // Start is called before the first frame update
    void Start()
    {
        CloneNextCar();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var roadLine in roadLines)
        {
            if (roadLine.GetComponent<RoadLine>().isReady())
            {
                roadLine.GetComponent<RoadLine>().StartNewCar(carClone);
                CloneNextCar();
            }
        }
    }

    private void CloneNextCar()
    {
        var nextCar = cars.transform.GetChild(0).gameObject;
        carClone = Instantiate(nextCar);
        carClone.transform.position = clonedCarsFirstSpawn.transform.position;
    }
}
