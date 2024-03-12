using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveLevelData : MonoBehaviour
{
    private int shotCount;
    public int ShotCount { get { return shotCount; } private set { shotCount = value; } }

    [SerializeField] TextMeshProUGUI shotCountText;

    //Just a placeholder way of increasing the shot count.
    //Replace with real stuff
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            shotCount++;
            shotCountText.text = shotCount.ToString();
        }
    }
}
