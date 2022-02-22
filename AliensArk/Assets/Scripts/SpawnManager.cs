/*
 * Jaden Pleasants
 * Project 2
 * Automatically spawns ailens every N turns
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // These can be played with in the inspector
    public int turnsBetweenSpawns;
    public int turnsBeforeFirstSpawn;
    public int maxAliensToSpawn;
    public GameObject ailenPrefab;

    private TurnManager TM;

    // This counter should automatically be reset as it's updeated
    private int _turnsSinceLastSpawn;
    private int turnsSinceLastSpawn
    {
        get => _turnsSinceLastSpawn; set
        {
            _turnsSinceLastSpawn = value % turnsBetweenSpawns;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        turnsSinceLastSpawn = 0;
        TM = TurnManager.GetTurnManager();
        TM.TurnEvent.AddListener(SpawnAlien);
    }

    void OnDestroy()
    {
        TM.TurnEvent.RemoveListener(SpawnAlien);
    }

    void SpawnAlien()
    {
        if (TM.currentTurn > turnsBeforeFirstSpawn && turnsSinceLastSpawn++ == 0)
        {
            Instantiate(ailenPrefab);
        }
    }
}
