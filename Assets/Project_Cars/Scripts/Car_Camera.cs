using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Camera : MonoBehaviour
{
    Camera _camera;
    public Vector3 _offset;
    private float defaultFOV;
    public float smooth;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _camera.transform.position = _offset;
        defaultFOV = _camera.fieldOfView;
    }

    private void LateUpdate()
    {
        Vector3 position = transform.localPosition;
        Vector3 smoothed = Vector3.Lerp(position, _offset, smooth);
        float distance = Vector3.Distance(position, _offset);
        transform.localPosition = position;
    }
}
