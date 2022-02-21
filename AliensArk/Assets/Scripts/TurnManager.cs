/*
 * Jaden Pleasants
 * Project 2
 * Event-based turn system
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    // Easier way to find the turn manager
    public static TurnManager GetTurnManager()
    {
        return GameObject.FindGameObjectWithTag("TurnSystem").GetComponent<TurnManager>();
    }

    private UnityEvent _TurnEvent = new UnityEvent();
    public UnityEvent TurnEvent
    {
        get { return _TurnEvent; }
    }

    // Turn counter (with a property for accessing it)
    private int _currentTurn = 0;
    public int currentTurn {
        get { return _currentTurn; }
    }

    public void EndTurn()
    {
        _currentTurn++;
        TurnEvent.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
