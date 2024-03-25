using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPGunController : MonoBehaviour
{
    public Transform Gun;
    public Transform Muzzle;

    public float Angle;
    public float Distance;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(Angle, 0.0f, 0.0f);
        Gun.localPosition = new Vector3(0.0f, 0.0f, -Distance);
    }
}
