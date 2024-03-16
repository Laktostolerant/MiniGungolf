using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveLevelData : MonoBehaviour
{
    private int shotCount;
    public int ShotCount { get { return shotCount; } private set { shotCount = value; } }

    [SerializeField] TextMeshProUGUI shotCountText;

    public static ActiveLevelData Instance;
    private void Awake() { Instance = this; }

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

    //This is a method that will be called when ball enters hole.
    //Needs a separation for multiplayer & singleplayer somehow?
    //Unsure so far how we should do it.
    public void BallEnteredHole()
    {

    }
}
