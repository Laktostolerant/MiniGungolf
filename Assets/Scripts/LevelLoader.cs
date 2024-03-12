using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance; 
    private void Awake() { instance = this; }

    int levelsLeftToLoad;

    List<Vector2> previouslyLoadedLevels;

    Vector2 levelToLoad;





    public void LoadSingleplayerLevel(Vector2 index)
    {

    }
}
