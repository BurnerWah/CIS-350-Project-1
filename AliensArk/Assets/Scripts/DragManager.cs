/*
 * Robert Krawczyk, Gerard Lamoureux
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
    private Slot currentSlot;

    // Called by a planet/slot hitbox where an alien is. Starts dragging the alien
    public void StartDragging(Alien alien, Slot from_slot)
    {
        if (alien != null && from_slot != null)
        {
            if (DEBUG)
                print("DragManager: Starting to drag " + alien.GetSpeciesName() + " from " + from_slot.name);
            dragging = true;
            alien_being_dragged = alien;
            startSlot = from_slot;
        }
    }

    // Called by the background hitbox. Stops dragging the alien and doesn't place it anywhere
    public void Drop()
    {
        if (DEBUG && alien_being_dragged != null)
            print("DragManager: Trying to drop " + alien_being_dragged.GetSpeciesName());
        if (dragging)
        {
            if (DEBUG && alien_being_dragged != null)
                print("DragManager: Dropping " + alien_being_dragged.GetSpeciesName());
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
        if (slot == null || alien_being_dragged == null)
        {
            return false;
        }
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
            print("DragManager: Placing " + alien.GetSpeciesName() + " into " + slot.name);
        alien.transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, -1); // Place in center of planet/slot

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
            //maybe just Input.mousePosition worked for you, but for me it wasn't linked to the Mouse - Gerard
            alien_being_dragged.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); // -100 to make sure it's shown on top
            alien_being_dragged.transform.position = new Vector3(alien_being_dragged.transform.position.x, alien_being_dragged.transform.position.y, -1);
        }
        
    }

    public void SetCurrentSlot(Slot slot)
    {
        currentSlot = slot;
    }

    public Slot GetCurrentSlot()
    {
        return currentSlot;
    }
}
