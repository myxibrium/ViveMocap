using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRotationMatcher : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}
