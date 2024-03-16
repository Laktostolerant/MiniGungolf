using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance; 
    private void Awake() { Instance = this; }

    int levelsLeftToLoad;

    List<Vector2> previouslyLoadedLevels;

    Vector2 levelToLoad;




    [ContextMenu("Load a level")]
    public void Test()
    {
        LoadSingleplayerLevel(new Vector2(0, 0));
    }

    public void LoadSingleplayerLevel(Vector2 index)
    {
        SceneManager.LoadScene(2);
        Debug.Log("index: " + index + " with prefab: " + LevelManager.Instance.GetIndexedLevel(index).levelPrefab);
        GameObject temp = Instantiate(LevelManager.Instance.GetIndexedLevel(index).levelPrefab, transform);
        Debug.Log(temp);

        Instantiate(new GameObject(), transform);
    }

    public void UpdateLevelHighscore(Vector2 index)
    {
        //LevelManager.instance.GetIndexedLevel(index).UpdateHighscore
    }

}
