/*
 * Jaden Pleasants
 * Project 2
 * Updates the turn counter in the UI.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCounterUI : MonoBehaviour
{
    private TurnManager TM;

    // Start is called before the first frame update
    void Start()
    {
        TM = TurnManager.GetTurnManager();
        TM.TurnEvent.AddListener(UpdateCounter);
        UpdateCounter();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateCounter()
    {
        gameObject.GetComponent<Text>().text = $"{TM.currentTurn}";
    }

    void OnDestroy()
    {
        TM.TurnEvent.RemoveListener(UpdateCounter);
    }
}
