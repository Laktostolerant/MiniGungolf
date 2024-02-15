using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * This one will take a while.
 * It's the lobby & netcode manager.
 * I got this one, ill add comments once it makes more sense.
 */

public class Manageroo : MonoBehaviour
{
    public static Manageroo Instance;
    private void Awake() { Instance = this; }

    int currentPlayerTurn;

    [SerializeField] List<Player> players;

    void Start()
    {
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.GetComponent<Player>());
        }
    }

    public void NextPlayerTurn()
    {
        if (players.Count == 1)
        {
            players[0].BecomePlayerTurn();
            return;
        }

        currentPlayerTurn++;
        players[currentPlayerTurn].BecomePlayerTurn();
        Debug.Log("started da new player turn lmao");
    }



    public void GetLobby()
    {

    }

    [SerializeField] public GameRegions[] gameRegions;

    [System.Serializable]
    public struct GameRegions
    {
        [SerializeField] public string name;
        [SerializeField] public List<GameObject> levels;
    }
}
