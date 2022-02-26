/*
 * Jaden Pleasants
 * Project 2
 * Automatically spawns ailens every N turns
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // These can be played with in the inspector
    public int turnsBetweenSpawns;
    public int turnsBeforeFirstSpawn;
    public int maxAliensToSpawn;
    public GameObject ailenPrefab;

    private TurnManager TM;
    private SlotManager SM;

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
        SM = GameObject.Find("/SlotManager").GetComponent<SlotManager>();
    }

    void OnDestroy()
    {
        TM.TurnEvent.RemoveListener(SpawnAlien);
    }

    void SpawnAlien()
    {
        // This is using LINQ, which is more or less SQL for C#
        // I don't know why it's a thing but eh it works.
        var viableSlots =
            from slot in SM.PlanetSlots
            let slotC = slot.GetComponent<Slot>()
            where slotC.alien == null && !slotC.IsHidden
            orderby Random.Range(0f, 1f) descending // This just sorts randomly
            select slot;
        if (TM.currentTurn > turnsBeforeFirstSpawn
            && turnsSinceLastSpawn++ == 0
            && viableSlots.Count() > 0)
        {
            var slot = viableSlots.First();
            var position = new Vector3(slot.transform.position.x, slot.transform.position.y, -1);
            var alien = Instantiate(ailenPrefab, position, ailenPrefab.transform.rotation);
            var slotC = slot.GetComponent<Slot>();
            var alienC = alien.GetComponent<Alien>();
            slotC.alien = alienC;
            alienC.slot = slotC;
        }
    }
}
