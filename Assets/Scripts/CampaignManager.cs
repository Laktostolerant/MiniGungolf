using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance;
    private void Awake() { Instance = this; }

    [SerializeField] GameObject levelsParentGrid;
    [SerializeField] TextMeshProUGUI worldTitle;
    [SerializeField] GameObject worldBackground;
    [SerializeField] Image worldDecor;
    List<GameObject> currentlyDisplayedLevels = new List<GameObject>();

    [Header("Level Info Display")]
    [SerializeField] GameObject levelInfoDisplay;
    [SerializeField] TextMeshProUGUI levelHighscoreDisplay;
    [SerializeField] TextMeshProUGUI levelIndexDisplay;
    [SerializeField] TextMeshProUGUI bronzeReqDisplay;
    [SerializeField] TextMeshProUGUI silverReqDisplay;
    [SerializeField] TextMeshProUGUI goldReqDisplay;

    int currentLevelDisplayIndex;

    public void EnableLevelsMenu()
    {
        currentLevelDisplayIndex = 0;
        DisplayIndexedWorld();
    }

    #region Alter Active World Index
    // Increases the displayed world index.
    public void IncreaseWorldIndex()
    {
        currentLevelDisplayIndex++;

        if (currentLevelDisplayIndex >= LevelStorage.Instance.Worlds.Length)
            currentLevelDisplayIndex = 0;

        DisplayIndexedWorld();
    }

    // Decreases the displayed world index.
    public void DecreaseWorldIndex()
    {
        currentLevelDisplayIndex--;

        if (currentLevelDisplayIndex < 0)
            currentLevelDisplayIndex = LevelStorage.Instance.Worlds.Length - 1;

        DisplayIndexedWorld();
    }
    #endregion

    //Sets up the display for the currently selected world.
    //Instantiates prefabs of the levels into a grid.
    //Allows for less than 16 levels but no more than it.
    public void DisplayIndexedWorld(int index = -1)
    {
        if (index == -1) index = currentLevelDisplayIndex;

        foreach (GameObject level in currentlyDisplayedLevels) Destroy(level);
        currentlyDisplayedLevels.Clear();

        World currentWorld = LevelStorage.Instance.Worlds[index];

        worldTitle.text = currentWorld.worldName;
        worldTitle.color = currentWorld.worldTitleColor;
        worldBackground.GetComponent<Image>().color = currentWorld.worldBackgroundColor;
        worldDecor.sprite = currentWorld.worldDecor;

        for (int i = 0; i < currentWorld.levels.Length; i++)
        {
            GameObject temp = Instantiate(currentWorld.levels[i].gameObject, levelsParentGrid.transform);
            temp.GetComponent<GameLevel>().SetupDisplay(i);
            currentlyDisplayedLevels.Add(temp);
        }
    }

    //Whenever you click on a level, it will send over its data.
    //The data is then fed into this, which enables the level viewer.
    //The level viewer's data is set to the clicked level.

    public void OnLevelDisplay(GameLevel levelData)
    {
        //This is for if you want to load a level, it knows which level to load.
        LevelStorage.Instance.selectedLevel = new Vector2(currentLevelDisplayIndex, levelData.worldLevelIndex);

        levelInfoDisplay.SetActive(true);
        levelIndexDisplay.text = "Level " + (1 + levelData.worldLevelIndex).ToString();
        levelHighscoreDisplay.text = "HIGHSCORE: " + levelData.highScore.ToString();

        bronzeReqDisplay.text = levelData.medalBronzeRequirement.ToString();
        silverReqDisplay.text = levelData.medalSilverRequirement.ToString();
        goldReqDisplay.text = levelData.medalGoldRequirement.ToString();
    }
}
