using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour
{
    public GameObject levelPrefab;
    public int medalBronzeRequirement;
    public int medalSilverRequirement;
    public int medalGoldRequirement;

    [Header("Playerstats.")]
    public int highScore;

    public int levelIndex;


    public GameLevel(GameObject level, int bronze, int silver, int gold, int highscore)
    {
        this.levelPrefab = level;
    }

    public void SetupDisplay(int index)
    {
        levelIndex = index + 1;
    }

    public void EnableLevelDisplay()
    {
        LevelManager.instance.OnLevelDisplay(this);
    }
}
