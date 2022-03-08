/*
 * Robert Krawczyk, Jaden Pleasants
 * Project 2
 * Chooses and locks planets
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilShip : MonoBehaviour
{
    public GameObject[] planets;
    Slot currentPlanet;
    TurnManager turnManager;
    bool firstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        turnManager = TurnManager.GetTurnManager();
        turnManager.TurnEvent.AddListener(NextTurn);
    }

    void OnDestroy()
    {
        turnManager.TurnEvent.RemoveListener(NextTurn);
    }

    void NextTurn()
    {
        if (turnManager.currentTurn % 5 == 1 )//&& turnManager.currentTurn != 1)
        {
            MoveToNextPlanet();
        }
    }

    void MoveToNextPlanet()
    {
        print($"Evil Ship moving to next planet");
        if (firstTime)
        {
            firstTime = false;
        }
        else
        {
            currentPlanet.Lock();
        }
        currentPlanet = planets[Random.Range(0, planets.Length)].GetComponent<Slot>();
        transform.position = currentPlanet.transform.position + Vector3.right * 3.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
