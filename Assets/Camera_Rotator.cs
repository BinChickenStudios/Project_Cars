using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Rotator : MonoBehaviour
{
    bool isRotate = false;
    float mouseX, mouseY;
    public float sensitivity = 10f;
    

    // Update is called once per frame
    void Update()
    {
        isRotate = Input.GetMouseButton(1);
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (isRotate)
        {
            transform.Rotate(Vector3.up * mouseX * sensitivity * Time.deltaTime);
            //transform.Rotate(Vector3.right * mouseY * sensitivity * Time.deltaTime);
        }
    }
}
