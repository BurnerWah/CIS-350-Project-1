/*
 * Robert Krawczyk, 
 * Project1
 * Controls the dragging of aliens from slot to slot. Some of this object's methods are called by Slots
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public bool DEBUG = true;

    public bool dragging;
    public Alien alien_being_dragged;
    private Slot startSlot; // The planet/slot the alien was on before it started being dragged

    // Called by a planet/slot hitbox where an alien is. Starts dragging the alien
    public void StartDragging(Alien alien, Slot from_slot)
    {
        if (alien != null && from_slot != null)
        {
            if (DEBUG)
                print("DragManager: Starting to drag " + alien.name + " from " + from_slot.name);
            dragging = true;
            alien_being_dragged = alien;
            startSlot = from_slot;
        }
        
    }

    // Called by the background hitbox. Stops dragging the alien and doesn't place it anywhere
    public void Drop()
    {
        if (DEBUG && alien_being_dragged != null)
            print("DragManager: Trying to drop " + alien_being_dragged.name);
        if (dragging)
        {
            if (DEBUG && alien_being_dragged != null)
                print("DragManager: Dropping " + alien_being_dragged.name);
            alien_being_dragged.transform.position = startSlot.transform.position; // Put back where it started
            dragging = false;
            alien_being_dragged = null;
        }
    }

    // Called by a planet/slot hitbox. Places the alien on that planet/slot
    public bool TryPlaceDragged(Slot slot)
    {
        if (DEBUG)
            print("DragManager: Trying to place the dragged into " + slot.name);
        if (dragging)
        {
            Place(alien_being_dragged, slot);
            if(startSlot != slot)
                startSlot.alien = null;
            dragging = false;
            alien_being_dragged = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Can be indirectly called by planet/slots via TryPlace(), or by this DragManager itself when setting the initial slots of aliens
    public void Place(Alien alien, Slot slot)
    {
        if (DEBUG)
            print("DragManager: Placing " + alien.name + " into " + slot.name);
        alien.transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, alien.transform.position.z); // Place in center of planet/slot

        slot.alien = alien;
        alien.slot = slot;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // While being dragged, make the object follow the mouse
        if (alien_being_dragged != null)
        {
            if (DEBUG)
                if (Time.time % .5 == 0) // twice per second
                    print("DragManager: Keeping " + alien_being_dragged + " with mouse X and Y");
            alien_being_dragged.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, alien_being_dragged.transform.position.z); // -100 to make sure it's shown on top
        }
        
    }
}
