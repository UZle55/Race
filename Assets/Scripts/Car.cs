using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float defaultSpeed;
    [SerializeField] private GameObject car;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {

        GetComponent<Rigidbody>().AddForce(0, 0, defaultSpeed);

    }
}
