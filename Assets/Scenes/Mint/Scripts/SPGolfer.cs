using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPGolfer : MonoBehaviour
{
    public Transform Pivot;
    public Transform Pointer;

    private void Update()
    {
        Pivot.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * 120.0f * Time.deltaTime, 0.0f));
        Pointer.rotation = Quaternion.Euler(90.0f, Pivot.eulerAngles.y, 0.0f);
    }
}
