using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float turnForce;
    [SerializeField] private float accelerationForce;
    [SerializeField] private KeyCode goLeftButton;
    [SerializeField] private KeyCode goRightButton;
    [SerializeField] private KeyCode accelerationButton;
    [SerializeField] private KeyCode decelerationButton;
    /*private bool isGoingLeft = false;
    private bool isGoingRight = false;
    private bool isAccelerate = false;
    private bool isDecelerate = false;*/
    private Vector2 direction = new Vector2();
    [SerializeField] private GameObject moto;
    [SerializeField] private float tiltAniSpeed;
    [SerializeField] private float tiltDegrees;
    private bool isAniTilt = false;
    [SerializeField] private GameObject info;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Tilt();
    }


    private void FixedUpdate()
    {

        GetComponent<Rigidbody>().AddForce(direction.x * turnForce, 0, defaultSpeed + direction.y * accelerationForce);
        
    }

    private void Tilt()
    {
        var currentAngle = moto.transform.parent.GetComponent<Transform>().localEulerAngles.z;
        if (currentAngle <= 90)
            currentAngle = 360 + currentAngle;
        var leftTiltDegrees = 360 + tiltDegrees;
        var rightTiltDegrees = 360 - tiltDegrees;
        var degreesToAdd = 0.0f;
        if (!isAniTilt)
        {
            if ((direction.x < 0 && currentAngle != leftTiltDegrees) 
                || (direction.x > 0 && currentAngle != rightTiltDegrees)
                || (direction.x == 0 && currentAngle != 360))
            {
                isAniTilt = true;
            }
        }
        if (isAniTilt)
        {
            if (direction.x < 0 && currentAngle < 360)
            {
                degreesToAdd = Time.deltaTime * tiltAniSpeed * 2;
            }
            if (direction.x > 0 && currentAngle > 360)
            {
                degreesToAdd = - Time.deltaTime * tiltAniSpeed * 2;
            }
            if (direction.x < 0 && currentAngle >= 360)
            {
                degreesToAdd = Time.deltaTime * tiltAniSpeed;
            }
            if (direction.x > 0 && currentAngle <= 360)
            {
                degreesToAdd = - Time.deltaTime * tiltAniSpeed;
            }
            if (direction.x == 0 && currentAngle < 360)
            {
                degreesToAdd = Time.deltaTime * tiltAniSpeed;
                if(currentAngle + degreesToAdd > 360)
                {
                    currentAngle = 360;
                    isAniTilt = false;
                }
            }
            if (direction.x == 0 && currentAngle > 360)
            {
                degreesToAdd = - Time.deltaTime * tiltAniSpeed;
                if (currentAngle + degreesToAdd < 360)
                {
                    currentAngle = 360;
                    isAniTilt = false;
                }
            }
            if (isAniTilt)
            {
                currentAngle += degreesToAdd;
                if (currentAngle > leftTiltDegrees)
                {
                    currentAngle = leftTiltDegrees;
                    isAniTilt = false;
                }
                if (currentAngle < rightTiltDegrees)
                {
                    currentAngle = rightTiltDegrees;
                    isAniTilt = false;
                }
            }
            moto.transform.parent.GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, currentAngle);
        }

    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(goLeftButton))
        {
            direction += new Vector2(-1, 0);
        }
        if (Input.GetKeyDown(goRightButton))
        {
            direction += new Vector2(1, 0);
        }
        if (Input.GetKeyDown(accelerationButton))
        {
            direction += new Vector2(0, 1);
        }
        if (Input.GetKeyDown(decelerationButton))
        {
            direction += new Vector2(0, -1);
        }

        if (Input.GetKeyUp(goLeftButton))
        {
            direction += new Vector2(1, 0);
        }
        if (Input.GetKeyUp(goRightButton))
        {
            direction += new Vector2(-1, 0);
        }
        if (Input.GetKeyUp(accelerationButton))
        {
            direction += new Vector2(0, -1);
        }
        if (Input.GetKeyUp(decelerationButton))
        {
            direction += new Vector2(0, 1);
        }
    }
}
