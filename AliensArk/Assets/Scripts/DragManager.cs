/*
 * Robert Krawczyk, Gerard Lamoureux, Jaden Pleasants Conner Ogle
 * Project1
 * Controls the dragging of aliens from slot to slot. Some of this object's methods are called by Slots
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public bool dragging;
    public Alien draggedAlien;
    private Slot startSlot; // The planet/slot the alien was on before it started being dragged
    private Slot _currentSlot;
    public Slot CurrentSlot { get => _currentSlot; set => _currentSlot = value; }

    // Called by a planet/slot hitbox where an alien is. Starts dragging the alien
    public void StartDragging(Alien alien, Slot from_slot)
    {
        if (alien != null && from_slot != null)
        {
            Debug.Log($"DragManager: Starting to drag {alien.SpeciesName} from {from_slot.name}");
            dragging = true;
            draggedAlien = alien;
            startSlot = from_slot;
        }
    }

    // Called by the background hitbox. Stops dragging the alien and doesn't place it anywhere
    public void Drop()
    {
        if (draggedAlien != null && dragging)
        {
            Debug.Log($"DragManager: Dropping {draggedAlien.SpeciesName}");
            draggedAlien.transform.position = startSlot.transform.position; // Put back where it started
            dragging = false;
            draggedAlien = null;
        }
    }

    // Called by a planet/slot hitbox. Places the alien on that planet/slot
    public bool TryPlaceDragged(Slot slot)
    {
        Debug.Log($"DragManager: Trying to place the dragged into {slot.name}");
        if (dragging && draggedAlien != null)
        {
            if (IsCompatible(draggedAlien, slot))
            {
                Place(draggedAlien, slot);
                if (startSlot != slot)
                {
                    startSlot.alien = null;
                }
                dragging = false;
                draggedAlien = null;
                return true;
            }
            dragging = false;
            draggedAlien = null;

        }
        return false;
    }

    // Can be indirectly called by planet/slots via TryPlace(), or by this DragManager itself when setting the initial slots of aliens
    public void Place(Alien alien, Slot slot)
    {
        Debug.Log($"DragManager: Placing {alien.SpeciesName} with {alien.GetAlienTerrain()}, {alien.GetAlienAtmo()}, and {alien.GetAlienResource()} into {slot.name}");
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
        if (draggedAlien != null)
        {
            if (Time.time % .5 == 0) // twice per second
            {
                Debug.Log($"DragManager: Keeping {draggedAlien} with mouse X and Y");
            }
            //maybe just Input.mousePosition worked for you, but for me it wasn't linked to the Mouse - Gerard
            draggedAlien.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); // -100 to make sure it's shown on top
            draggedAlien.transform.position = new Vector3(draggedAlien.transform.position.x, draggedAlien.transform.position.y, -1);
        }

    }

    public bool IsCompatible(Alien alien, Slot slot)
    {

        if (alien.Terrain == slot.Terrain || alien.Temp == slot.Temp)
        {
            return true;
        }
        else if (slot.Terrain == "Ship" && slot.Temp == "Ship")
        {
            return true;
        }
        return false;
    }
}
