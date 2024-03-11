using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Stores data for the worlds.
 * Contains world color for the title, world name for the display name.
 * Also contains the different levels in the world.
 *
*/

[System.Serializable]
public class World
{
    public Color worldColor;
    public string worldName;
    public GameLevel[] levels;

    public World(Color color, string name, GameLevel[] levels)
    {
        this.worldColor = color;
        this.worldName = name;
        this.levels = levels;
    }
}
