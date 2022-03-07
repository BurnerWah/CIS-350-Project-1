/*
 * Jaden Pleasants, Robert Krawczyk
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
    public Text scoreText, highScoreText;
    public int turnsUntilWin;

    private TurnManager TM;
    private SpawnManager spawnManager;
    public SlotManager slotManager;

    public int aliensDestroyed;
    int score = 0;


    void OnEndTurn()
    {
        // Add score and update score text
        foreach(GameObject slotGameobject in slotManager.AllSlots)
        {
            Alien alien = slotGameobject.GetComponent<Slot>().alien;
            if(alien != null)
            {
                score += alien.Happiness;
            }
            
        }
        scoreText.text = $"Score: {score.ToString()}";

        // Check if won/lost
        var gameOver = false;
        var won = false;
        if (aliensDestroyed >= spawnManager.maxAliensToSpawn - 1) // Lost
        {
            gameOver = true;
            won = false;
            Globals.curr_highScore = 1;
        }
        else if (TM.currentTurn >= turnsUntilWin) // Won
        {
            gameOver = true;
            won = true;
            if (score > Globals.curr_highScore)
            {
                Globals.curr_highScore = score;
            }
        }
        if (gameOver)
        {
            gameOverText.text = $"You {(won ? "won" : "lost")}!\nPress R to restart";
            gameOverText.enabled = true;
            gameOverText.gameObject.SetActive(true);
            highScoreText.text = $"High Score: {Globals.curr_highScore}";
            // Stop all future turn-related events
            TM.TurnEvent.RemoveAllListeners();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        aliensDestroyed = 0;
        TM = TurnManager.GetTurnManager();
        TM.TurnEvent.AddListener(OnEndTurn);
        spawnManager = GameObject.Find("/SpawnManager").GetComponent<SpawnManager>();
        gameOverText.gameObject.SetActive(false);
        highScoreText.text = $"High Score: {Globals.curr_highScore}";
        scoreText.text = $"Score: {score.ToString()}";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int addToScore)
    {
        score += addToScore;
    }
}
