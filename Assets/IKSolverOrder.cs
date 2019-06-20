using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class IKSolverOrder : MonoBehaviour
{

    public IK[] components;

    void Start() {
        foreach (IK component in components) component.Disable();
    }
    void LateUpdate() {
        foreach (IK component in components) component.GetIKSolver().Update();
    }
    
}
