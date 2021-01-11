using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Steering : MonoBehaviour
{
    private Rigidbody rb;
    private Car_Body body;
    private Car_Engine engine;

    public float turnRadius = 15f;

    public bool frontWheel = true;
    private float front = 1;
    public bool PassangerWheel = true;
    private float passanger = 1;

    private float wheelAngle = 0;

    private void Start()
    {
        body = transform.root.GetComponent<Car_Body>();
        engine = transform.root.GetComponent<Car_Engine>();
        rb = transform.root.GetComponent<Rigidbody>();
        
        front = frontWheel ? 1 : - 1;
        passanger = PassangerWheel ? -1 : 1;
    }


    private void Update()
    {
        wheelAngle = CalculateAngle();
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + wheelAngle, transform.localRotation.z);
        rb.AddForceAtPosition(transform.forward * engine.speed * engine.accelerationInput * Time.fixedDeltaTime, transform.position);
    }

    float CalculateAngle() 
    {
        float angle;

        float _wheelBase = body.wheelBase * front;
        float _rearTrack = body.rearTrack * passanger;

        if (engine.steerInput > 0)
        {
            angle = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (turnRadius + (_rearTrack / 2))) * engine.steerInput;
        }
        else if (engine.steerInput < 0)
        {
            angle = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (turnRadius - (_rearTrack / 2))) * engine.steerInput;
        }
        else 
        {
            angle = 0;
        }


        return angle;
    }
}
