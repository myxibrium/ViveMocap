using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Valve.VR;

public class RigCalibrator : MonoBehaviour
{
    public Transform ikControlsParent;

    public bool calibrate;

    private void Update()
    {
        if (calibrate)
        {
            Calibrate();

            calibrate = false;
        }
    }

    public void Calibrate()
    {
        Debug.Log("Calibrate called.");
        
        var numIkControls = ikControlsParent.childCount;
        var numMocapControls = transform.childCount;
        
        NativeArray<float3> ikControlPositions = new NativeArray<float3>(numIkControls, Allocator.TempJob);
        NativeArray<quaternion> ikControlRotations = new NativeArray<quaternion>(numIkControls, Allocator.TempJob);

        NativeArray<float3> mocapPositions = new NativeArray<float3>(numMocapControls, Allocator.TempJob);
        NativeArray<quaternion> mocapRotations = new NativeArray<quaternion>(numMocapControls, Allocator.TempJob);

        NativeArray<float3> mocapOffsetPositions = new NativeArray<float3>(numMocapControls, Allocator.TempJob);
        NativeArray<quaternion> mocapOffsetRotations = new NativeArray<quaternion>(numMocapControls, Allocator.TempJob);

        for (int i = 0; i < numIkControls; i++)
        {
            var child = ikControlsParent.GetChild(i);
            ikControlPositions[i] = child.position;
            ikControlRotations[i] = child.rotation;
        }

        for (int i = 0; i < numMocapControls; i++)
        {
            var child = transform.GetChild(i);
            mocapPositions[i] = child.position;
            mocapRotations[i] = child.rotation;
        }


        for (int i = 0; i < numMocapControls; i++)
        {
            float3 mocapPos = mocapPositions[i];
            int nearest = findNearest(mocapPos, ikControlPositions);
            mocapOffsetPositions[i] = ikControlPositions[nearest];
            mocapOffsetRotations[i] = math.mul(math.inverse(mocapRotations[i]), ikControlRotations[nearest]);
            
            var ikTarget = ikControlsParent.GetChild(nearest).GetComponent<PositionRotationMatcher>();
            if (ikTarget)
            {
                ikTarget.target = transform.GetChild(i);
            }
        }


        for (int i = 0; i < numMocapControls; i++)
        {
            var mocapTarget = transform.GetChild(i).GetChild(0);
            mocapTarget.position = mocapOffsetPositions[i];
            mocapTarget.rotation = mocapOffsetRotations[i];
        }

        ikControlPositions.Dispose();
        ikControlRotations.Dispose();
        mocapPositions.Dispose();
        mocapRotations.Dispose();
        mocapOffsetPositions.Dispose();
        mocapOffsetRotations.Dispose();
    }
    
    private static int findNearest(float3 search, NativeArray<float3> values)
    {
        int index = -1;
        float currentLenSq = float.MaxValue;

        for (int i = 0; i < values.Length; i++)
        {
            float3 value = values[i];
            float lenSq = math.lengthsq(value - search);
            if (lenSq < currentLenSq)
            {
                index = i;
                currentLenSq = lenSq;
            }
        }

        return index;
    }
}
