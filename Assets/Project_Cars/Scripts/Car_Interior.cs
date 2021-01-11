using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Interior : MonoBehaviour
{
    Car_Engine engine;
    public Transform steeringWheel;
    public float revolutions = 1.5f;
    private Quaternion startRotation;

    private void Start()
    {
        engine = transform.root.GetComponent<Car_Engine>();
        startRotation = steeringWheel.localRotation;
    }

    private void Update()
    {
        float angle = engine.steerInput * (revolutions * 360);

        Quaternion rotation = startRotation;

        Vector3 euler = rotation.eulerAngles;

        euler.y = angle;

        rotation.eulerAngles = euler;

        steeringWheel.localRotation = rotation;
    }
}
