using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStorage : MonoBehaviour
{
    public static LevelStorage Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] World[] worlds;

    public World[] Worlds { get { return worlds; } private set { worlds = value; } }

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
    //Idk if this will be needed?
    //Just returns a specific level
    public GameLevel GetIndexedLevel(Vector2 index)
    {
        return worlds[(int)index.x].levels[(int)index.y];
    }

    //Just a simple "grab random level" thing.
    public GameLevel GetRandomLevel()
    {
        GameLevel temp;
        int randomWorld = Random.Range(0, worlds.Length);
        temp = worlds[randomWorld].levels[Random.Range(0, worlds[randomWorld].levels.Length)];

        return temp;
    }

    public void OnPlayButton()
    {
        LevelLoader.Instance.LoadSingleplayerLevel(selectedLevel);
    }
}
