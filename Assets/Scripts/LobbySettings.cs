using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySettings : MonoBehaviour
{
    public enum GameModeTypes { RANDOM, SPECIFIC, CUSTOM };

    public GameModeTypes GameMode;

    List<bool> enabledRegions;
}
