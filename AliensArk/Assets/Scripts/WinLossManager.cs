/*
 * Jaden Pleasants
 * Project 2
 * Manages win/loss conditions
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLossManager : MonoBehaviour
{
    public Text gameOverText;
    public int turnsUntilWin;

    private TurnManager TM;
    private SpawnManager spawnManager;

    public int aliensDestroyed;


    void CheckIfGameEnded()
    {
        var gameOver = false;
        var won = false;
        if (aliensDestroyed >= spawnManager.maxAliensToSpawn + 1)
        {
            gameOver = true;
            won = false;
        }
        else if (TM.currentTurn >= turnsUntilWin)
        {
            gameOver = true;
            won = true;
        }
        if (gameOver)
        {
            gameOverText.text = $"You {(won ? "won" : "lost")}!\nPress R to restart";
            gameOverText.enabled = true;
            gameOverText.gameObject.SetActive(true);
            // Stop all future turn-related events
            TM.TurnEvent.RemoveAllListeners();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        aliensDestroyed = 0;
        TM = TurnManager.GetTurnManager();
        TM.TurnEvent.AddListener(CheckIfGameEnded);
        spawnManager = GameObject.Find("/SpawnManager").GetComponent<SpawnManager>();
        gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
