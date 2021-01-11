using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Engine : MonoBehaviour
{
    public float mass;
    public Vector3 centerOfMass = Vector3.zero;
    public float speed = 10f;
    public Car_Gears gears;
    [HideInInspector] public float steerInput;
    [HideInInspector] public float accelerationInput;


    private void Update()
    {
        steerInput = Input.GetAxis("Horizontal");
        accelerationInput = Input.GetAxis("Vertical");
    }


    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass += mass;
        rb.centerOfMass = centerOfMass;

    }

    private void FixedUpdate()
    {
        rb.centerOfMass = centerOfMass;
    }
}
