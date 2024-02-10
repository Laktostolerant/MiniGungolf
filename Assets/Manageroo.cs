using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
