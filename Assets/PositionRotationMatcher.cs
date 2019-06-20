using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRotationMatcher : MonoBehaviour
{
    public Transform target;

    public bool matchPosition = true;

    public bool matchRotation = true;

    public bool lateUpdate = true;

    private void Update()
    {
        if (!lateUpdate)
        {
            match();
        }
    }

    private void LateUpdate()
    {
        if (lateUpdate)
        {
            match();
        }
    }

    private void match()
    {
        if (target)
        {
            if (matchPosition)
            {
                transform.position = target.position;
            }

            if (matchRotation)
            {
                transform.rotation = target.rotation;
            }
        }
    }
}
