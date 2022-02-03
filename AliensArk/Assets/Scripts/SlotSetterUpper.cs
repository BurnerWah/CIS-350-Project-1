/*
 * Robert Krawczyk, 
 * Project1
 * At scene load, places aliens into slots they are near
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSetterUpper : MonoBehaviour
{
    // Scene objects
    public GameObject aliens;
    public GameObject planets;
    public GameObject ark;
    DragManager dragManager;

    // Private variables
    float snapDistance = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Find objects
        dragManager = Object.FindObjectOfType<DragManager>();

        for (int i = 0; i < aliens.transform.childCount; i++)
        {
            Alien alien = aliens.transform.GetChild(i).GetComponent<Alien>();
            bool found = false;
            // Check the planets and then check the ark slots
            for(int j = 0; j < planets.transform.childCount; j++)
            {
                Slot slot = planets.transform.GetChild(j).GetComponent<Slot>();
                if(Vector3.Distance(alien.transform.position, slot.transform.position) <= snapDistance) // If alien is close enough to slot
                {
                    // Place alien in slot
                    dragManager.Place(alien, slot);
                    found = true;
                    break;
                }
            }
            // Then check the ark slots for aliens
            if (!found)
            {
                for (int j = 0; j < ark.transform.childCount; j++)
                {
                    Slot slot = planets.transform.GetChild(j).GetComponent<Slot>();
                    if (Vector3.Distance(alien.transform.position, slot.transform.position) <= 5) // If alien is close enough to slot
                    {
                        // Place alien in slot
                        dragManager.Place(alien, slot);
                        found = true;
                        break;
                    }
                }
            }
            // If the current alien was not close enough to a slot, deactivate it
            if (!found)
            {
                print(alien.name + " was not close enough to a slot, skipped");
                alien.gameObject.SetActive(false);
            }


        }

        // Kills self when done
        //Destroy(gameObject);
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
