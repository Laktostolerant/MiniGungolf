using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterMover : MonoBehaviour
{
    [SerializeField] Image img;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTheWater());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveTheWater()
    {
        for(int i = 0; i < 1620; i++)
        {
            img.transform.position += new Vector3(0, 2);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
