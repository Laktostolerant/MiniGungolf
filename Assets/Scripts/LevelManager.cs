using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] World[] worlds;
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

    public Vector2 selectedLevel;

    public Vector2 GetCurrentLevel { get { return selectedLevel; } private set { selectedLevel = value; }
}
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

        if(currentLevelDisplayIndex  >=worlds.Length)
            currentLevelDisplayIndex = 0;

        DisplayIndexedWorld();
    }

    // Decreases the displayed world index.
    public void DecreaseWorldIndex()
    {
        currentLevelDisplayIndex--;

        if (currentLevelDisplayIndex < 0)
            currentLevelDisplayIndex = worlds.Length - 1;

        DisplayIndexedWorld();
    }
    #endregion


    //Sets up the display for the currently selected world.
    //Instantiates prefabs of the levels into a grid.
    //Allows for less than 16 levels but no more than it.
    public void DisplayIndexedWorld()
    {
        foreach(GameObject level in currentlyDisplayedLevels) Destroy(level);
        currentlyDisplayedLevels.Clear();

        worldTitle.text = worlds[currentLevelDisplayIndex].worldName;
        worldTitle.color = worlds[currentLevelDisplayIndex].worldTitleColor;
        worldBackground.GetComponent<Image>().color = worlds[currentLevelDisplayIndex].worldBackgroundColor;
        worldDecor.sprite = worlds[currentLevelDisplayIndex].worldDecor;

        for (int i = 0; i < worlds[currentLevelDisplayIndex].levels.Length; i++)
        {
            GameObject temp = Instantiate(worlds[currentLevelDisplayIndex].levels[i].gameObject, levelsParentGrid.transform);
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
        selectedLevel = new Vector2(currentLevelDisplayIndex, levelData.levelIndex);

        levelInfoDisplay.SetActive(true);
        levelIndexDisplay.text = "Level " + levelData.levelIndex.ToString();
        levelHighscoreDisplay.text = "HIGHSCORE: " + levelData.highScore.ToString();

        bronzeReqDisplay.text = levelData.medalBronzeRequirement.ToString();
        silverReqDisplay.text = levelData.medalSilverRequirement.ToString();
        goldReqDisplay.text = levelData.medalGoldRequirement.ToString();
    }

    //Idk if this will be needed?
    //Just returns a specific level
    public GameLevel GetIndexedLevel(Vector2 index)
    {
        return worlds[(int)index.x].levels[(int)index.y];
    }
}
