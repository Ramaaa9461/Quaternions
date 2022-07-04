using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot3d : MonoBehaviour
{
    [SerializeField] Vector3 angle = Vector3.zero;

    void Update()
    {
        float real;
        float imaginary;


        imaginary = Mathf.Sin(Mathf.Deg2Rad * angle.x / 2.0f);
        real = Mathf.Cos(Mathf.Deg2Rad * angle.x / 2.0f);

        Quaternion rotX = Quaternion.identity;
        rotX.w = real;
        rotX.x = imaginary;


        imaginary = Mathf.Sin(Mathf.Deg2Rad * angle.y / 2.0f);
        real = Mathf.Cos(Mathf.Deg2Rad * angle.y / 2.0f);

        Quaternion rotY = Quaternion.identity;
        rotY.w = real;
        rotY.y = imaginary;


        imaginary = Mathf.Sin(Mathf.Deg2Rad * angle.z / 2.0f);
        real = Mathf.Cos(Mathf.Deg2Rad * angle.z / 2.0f);

        Quaternion rotZ = Quaternion.identity;
        rotZ.w = real;
        rotZ.z = imaginary;

        transform.rotation = (rotX * rotY * rotZ);
    }
}
