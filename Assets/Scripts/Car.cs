using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float defaultSpeed;
    [SerializeField] private GameObject car;
    [SerializeField] private bool isForwardDirection;
    [SerializeField] private bool isExample;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        if (!isExample)
        {
            if (isForwardDirection)
            {
                GetComponent<Rigidbody>().AddForce(0, 0, defaultSpeed);
            }
            else
            {
                GetComponent<Rigidbody>().AddForce(0, 0, -defaultSpeed);
            }
        }
    }

    public void Activate(bool isForward)
    {
        isForwardDirection = isForward;
        if (!isForward)
        {
            transform.GetChild(0).localEulerAngles += new Vector3(0, 180, 0);
        }
        isExample = false;
    }
}
