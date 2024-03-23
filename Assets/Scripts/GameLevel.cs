using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour
{
    public Vector3 spawnPoint;

    public GameObject levelPrefab;
    public int medalBronzeRequirement;
    public int medalSilverRequirement;
    public int medalGoldRequirement;

    [Header("Playerstats.")]
    public int highScore;

    public int worldLevelIndex;

    [SerializeField] Transform spawnLocation;

    public GameLevel(GameObject level, int bronze, int silver, int gold, int highscore)
    {
        this.levelPrefab = level;
    }

    public void SetupDisplay(int level)
    {
        worldLevelIndex = level;
    }

    //Display the level's stats when selected in the main menu.
    //This is the method that is called when pressing a level button in the menu.
    public void EnableLevelDisplay()
    {
        CampaignManager.Instance.OnLevelDisplay(this);
    }

    //Set this level's highscore to the next best if it's better.
    public void UpdateHighscore(int score)
    {
        if(score < highScore)
        {
            highScore = score;
        }
    }
}
