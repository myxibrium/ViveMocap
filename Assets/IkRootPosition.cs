using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkRootPosition : MonoBehaviour
{

    public Transform headIkControl;
    
    public Transform bodyIkControl;

    public Transform floorReference;

    private float initialBodyIkRotation;

    private void Start()
    {
        initialBodyIkRotation = bodyIkControl.rotation.y;
    }

    void LateUpdate()
    {
        var ave = (headIkControl.position + bodyIkControl.position) / 2;
        transform.position = new Vector3(ave.x, floorReference?floorReference.position.y:0, ave.z);
        var yrot = bodyIkControl.rotation.y - initialBodyIkRotation;
        transform.rotation = Quaternion.Euler(0, yrot, 0);
    }
}
