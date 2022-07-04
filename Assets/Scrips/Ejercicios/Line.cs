using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Line : MonoBehaviour
{
    [SerializeField] GameObject target;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Vector3.zero, target.transform.position);
    }
}
