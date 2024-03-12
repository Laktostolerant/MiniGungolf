using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Color worldTitleColor;
    public Color worldBackgroundColor;
    public Sprite worldDecor;
    public string worldName;
    public GameLevel[] levels;

    public World(Color titlecolor, Color backgroundcolor, string name, GameLevel[] levels)
    {
        this.worldTitleColor = titlecolor;
        this.worldBackgroundColor = backgroundcolor;
        this.worldName = name;
        this.levels = levels;
    }
}
