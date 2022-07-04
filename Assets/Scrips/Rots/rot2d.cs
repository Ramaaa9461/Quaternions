using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot2d : MonoBehaviour
{
    Vector3 rot = Vector3.zero;
    [SerializeField] float angle = 2.0f;
    [SerializeField] [Range(0, 1)] int orientation;

    void Update()
    {
        if (orientation == 0)
        {
            rot = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0.0f);
        }
        else if (orientation == 1)
        {
            rot = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), -Mathf.Sin(Mathf.Deg2Rad * angle), 0.0f);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = new Vector3(transform.position.x * rot.x - transform.position.y * rot.y,
                                             transform.position.y * rot.x + transform.position.x * rot.y,
                                             0.0f);
        }

    }
}
