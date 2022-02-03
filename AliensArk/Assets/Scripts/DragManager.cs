using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public bool dragging;
    public Alien alien_being_dragged;
    private Slot startSlot; // The planet/slot the alien was on before it started being dragged

    // Called by a planet/slot hitbox where an alien is. Starts dragging the alien
    public void StartDragging(Alien alien, Slot from_slot)
    {
        if(alien != null && from_slot != null)
        {
            dragging = true;
            alien_being_dragged = alien;
            startSlot = from_slot;
        }
        
    }

    // Called by the background hitbox. Stops dragging the alien and doesn't place it anywhere
    public void Drop()
    {
        if (dragging)
        {
            alien_being_dragged.transform.position = startSlot.transform.position; // Put back where it started
            dragging = false;
            alien_being_dragged = null;
        }
    }

    // Called by a planet/slot hitbox. Places the alien on that planet/slot
    public bool TryPlace(Slot slot)
    {
        if (dragging)
        {
            Place(slot);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Can be indirectly called by planet/slots via TryPlace(), or by this DragManager itself when setting the initial slots of aliens
    private void Place(Slot slot)
    {
        alien_being_dragged.transform.position = slot.transform.position; // Place in center of planet/slot
        startSlot.alien = null;
        slot.alien = alien_being_dragged;
        alien_being_dragged.slot = slot;
        dragging = false;
        alien_being_dragged = null;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // While being dragged, make the object follow the mouse
        if(alien_being_dragged != null)
        {
            alien_being_dragged.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100); // -100 to make sure it's shown on top
        }
        
    }
}
