using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : NetworkBehaviour
{
    public static LevelLoader Instance;
    private void Awake()
    {
        Instance = this;
    }

    GameLevel selectedSingleplayerLevel;
    Vector2 selectedSingleplayerIndex;

    int levelsLeftToLoad;

    List<Vector2> previouslyLoadedLevels;

    Vector2 levelToLoad;




    [ContextMenu("Load a level")]
    public void Test()
    {
        LoadSingleplayerLevel(new Vector2(0, 0));
    }

    //Goes into the play scene & loads the select level
    public void LoadSingleplayerLevel(Vector2 index)
    {
        GameLevel level = LevelStorage.Instance.GetIndexedLevel(index);

        SceneManager.LoadScene(2);
        GameObject temp = Instantiate(level.levelPrefab, transform);
    }

    //Work in progress once multiplayer is properly implemented.
    [ContextMenu("SELECT LEVELS LOL")]
    public void SpawnMultiplayerLevels()
    {
        List<GameLevel> gameLevels = new List<GameLevel>();

        while (gameLevels.Count < 5)
        {
            GameLevel tentativeLevel = LevelStorage.Instance.GetRandomLevel();
            bool levelAlreadyAdded = false;

            foreach(GameLevel level in gameLevels)
            {
                if (tentativeLevel == level)
                {
                    levelAlreadyAdded = true;
                    break;
                }
            }

            if (!levelAlreadyAdded)
            {
                Debug.Log("added a level");
                gameLevels.Add(tentativeLevel);
            }
        }

        Debug.Log("levels: " + gameLevels[0] + " " + gameLevels[1] + " " + gameLevels[2] + " " + gameLevels[3] + " " + gameLevels[4]);
    }

    //When back in the main menu, update the singleplayer level's highscore.
    public void UpdateLevelHighscore()
    {
        LevelStorage.Instance.GetIndexedLevel(selectedSingleplayerIndex).UpdateHighscore(1);
    }

}
