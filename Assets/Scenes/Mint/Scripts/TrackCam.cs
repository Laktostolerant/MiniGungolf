using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCam : MonoBehaviour
{
    public Transform Target;

    void Update()
    {
        Target.LookAt(Target.position);
    }
}
