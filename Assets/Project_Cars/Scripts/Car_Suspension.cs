using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Suspension : MonoBehaviour
{
    
    public Car_Spring spring;                      // spring data
    public Car_Wheel wheel;                        // wheel data
    
    private float lastLength;                      // spring length of previous physics update
    private float springLength;                    // spring length of current physics update

    private float springForce;                     // the amount of spring force to apply to the car
    private float damperForce;                     // the amount of damper force to apply to the car
    private float suspensionForce;                 // the combined forces to apply to the car

    private float springVelocity;                  // the springs current velocity 
    private Vector3 suspensionVelocity;            // the amount of suspension force to apply to the car

    [SerializeField]
    private LayerMask layermask;                   // the types of objects that can collide with the wheel
    private RaycastHit hit;                        // the object (floor) being hit by the wheel raycast
    private bool contact;                          // whether there is contact with an object (floor)

    private Rigidbody rb;                          // the cars rigidbody (to apply forces to)
    private Car_Body body;
    private Car_Engine engine;                     
    private Transform wheelObject;                 // the transform information of the wheel mesh 
    private Vector3 wheelLinearVelocityLocal;
    private float frictionForceX;
    private float frictionForceY;
    private float frictionForceZ;
    private Vector3 frictionForce;

    private void Start()
    {
        SetComponents();
        SetSuspensionHeight();
        ResetSpringLength();
    }
    private void SetComponents() 
    {
        rb = transform.root.GetComponent<Rigidbody>();
        body = transform.root.GetComponent<Car_Body>();
        engine = transform.root.GetComponent<Car_Engine>();
        wheelObject = transform.GetChild(0);
    }
    private void SetSuspensionHeight() 
    {
        Vector3 _position = transform.localPosition;
        _position.y = body.suspensionHeight;
        transform.localPosition = _position;
    }
    private void ResetSpringLength() 
    {
        springLength = spring.restLength;
    }

    private void Update()
    {
        Raycast();
        UpdateSpringLength();
        SetSpringForces();
        SetWheelPosition();
        SetFrictionForces();
        SetWheelRotation();
    }
    private void Raycast() 
    {
        //shoot a raycast from the suspension point
        contact = Physics.Raycast(transform.position, -transform.up, out hit, spring.maxLength + wheel.radius);
    }
    private void UpdateSpringLength() 
    {
        lastLength = springLength;

        if (contact)
        {
            springLength = hit.distance - wheel.radius;
            springLength = Mathf.Clamp(springLength, 0, spring.maxLength);
        }
        else
        {
            springLength = spring.maxLength;
        }

        springVelocity = (lastLength - springLength) / Time.deltaTime;
    }
    private void SetSpringForces() 
    {
        //calculate forces to apply to the car
        springForce = spring.stiffness * (spring.restLength - springLength);
        damperForce = spring.damper * springVelocity;

        suspensionForce = Mathf.Clamp(springForce + damperForce, spring.minForce, spring.maxForce);
        suspensionVelocity = suspensionForce * transform.up;
    }
    private void SetWheelPosition() 
    {
        //update the wheel position
        Vector3 wheelPosition = wheelObject.localPosition;
        wheelPosition.y = -springLength;
        wheelObject.localPosition = wheelPosition;
    }
    private void SetWheelRotation() 
    {
        //set and rotate wheels
        float rotationRadian = (wheelLinearVelocityLocal.z / (wheel.radius)) * Time.deltaTime;
        float rotationDegrees = rotationRadian * Mathf.Rad2Deg;
        wheelObject.GetChild(0).Rotate(new Vector3(rotationDegrees, 0, 0), Space.Self);
    }
    private void SetFrictionForces()
    {
        if (contact)
        {
            float friction;
            Vector3 _wheelPoint = wheelObject.position - (transform.up * wheel.radius);
            Vector3 wheelLinearVelocityWorld = rb.GetRelativePointVelocity(_wheelPoint);
            wheelLinearVelocityLocal = transform.InverseTransformDirection(wheelLinearVelocityWorld);

            PhysicMaterial _physicMaterial;
            bool hasFriction = hit.collider.TryGetComponent<PhysicMaterial>(out _physicMaterial);
            if (hasFriction) friction = _physicMaterial.staticFriction;
            else friction = 1f;

            frictionForce = wheelLinearVelocityLocal - (Vector3.forward * friction);
        }
        else
        {
            frictionForce = Vector3.zero;
        }
    }
    
    private void FixedUpdate()
    {
        ApplySpringForces();
        ApplyFrictionForces();
    }
    private void ApplySpringForces() 
    {
        //add the forces to the car
        rb.AddForceAtPosition(suspensionVelocity, transform.position);
    }
    private void ApplyFrictionForces()
    {      
        //apply friction forces
        if (contact)
        {
            rb.AddForceAtPosition(frictionForce, wheelObject.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - (transform.up * springLength));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - (transform.up * springLength), transform.position - (transform.up * (springLength + wheel.radius)));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position - new Vector3(0,0,-.5f), transform.position - new Vector3(0, spring.maxLength + wheel.radius, -.5f));

        Gizmos.color = Color.magenta;
        if (contact)
        {
            Gizmos.DrawCube(hit.point, Vector3.one * .1f);
        }
    }
}
