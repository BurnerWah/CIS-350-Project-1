/*
 * Jaden Pleasants, Robert Krawczyk
 * Project 2
 * Event-based turn system
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    // Easier way to find the turn manager
    public static TurnManager GetTurnManager()
    {
        return GameObject.FindGameObjectWithTag("TurnSystem").GetComponent<TurnManager>();
    }

    private UnityEvent _TurnEvent = new UnityEvent();
    public UnityEvent TurnEvent => _TurnEvent;

    // Turn counter (with a property for accessing it)
    private int _currentTurn = 1;
    public int currentTurn => _currentTurn;

    public Text txt_turnCounter;

    public void EndTurn()
    {
        _currentTurn++;
        txt_turnCounter.text = _currentTurn.ToString();
        TurnEvent.Invoke();
    }

    public void Reset()
    {
        _currentTurn = 1;
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
