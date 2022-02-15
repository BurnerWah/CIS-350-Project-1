/*
 * Robert Krawczyk, Gerard Lamoureux, Jaden Pleasants Conner Ogle
 * Project1
 * Controls hovering and hiding graphics, and keeps a reference to this slot's alien
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    // Object references
    Popup popup;
    DragManager dragManager;
    SpriteRenderer hover; // the white glow on hover
    SpriteRenderer hide; // the black layer when a planet has not been discovered

    // Public variables
    public Alien alien; // can only hold one
    public bool hidden; // Check this in the Unity scene for this slot to start hidden
    public string Terrain;
    public string Temp;

    // Private variables
    private Color hoverDragColor;
    private Color hoverNormalColor;
    private Color hideOldColor;


    // Start is called before the first frame update
    void Start()
    {
        // Search Scene
        dragManager = Object.FindObjectOfType<DragManager>();

        popup = GetComponentInChildren<Popup>(); // Gets the first popup child

        // Start up the hovering glow object
        // First child should be the hover object
        var hoverGameObject = transform.GetChild(0).gameObject;
        hover = hoverGameObject.GetComponent<SpriteRenderer>();
        hoverDragColor = hover.color;
        hoverNormalColor = hoverDragColor * new Color(1, 1, 1, 0.5f);
        hover.color = Color.clear;

        // Start up the hiding object
        var hideGameObject = gameObject.transform.GetChild(1).gameObject; // Second child should be the hide object. Ship slots are never hidden so they will have a placeholder instead
        hide = hideGameObject.GetComponent<SpriteRenderer>();
        hideOldColor = hide.color;
        hide.color = Color.clear;

        // Start hidden or not, based on whether you check the 'hidden' checkbox in the unity editor
        if (hidden)
        {
            Hide();
        }
        else
        {
            Discover();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alien != null)
        {
            alien.transform.position = new Vector3(transform.position.x, transform.position.y, -1); // Always snap my alien to me, just in case
        }
    }

    // When mouse enters the hitbox
    private void OnMouseEnter()
    {
        if (!hidden)
        {
            // Display the resource popup
            if (popup != null)
            {
                popup.GoUp();
            }

            // Display the white glow
            hover.color = dragManager.dragging ? hoverDragColor : hoverNormalColor;
            dragManager.SetCurrentSlot(this);
        }

    }

    // When mouse exits the hitbox
    private void OnMouseExit()
    {
        if (!hidden)
        {
            // Hide the resource popup
            if (popup != null)
            {
                popup.GoDown();
            }

            // Hide the white glow
            dragManager.SetCurrentSlot(null);
            hover.color = Color.clear;
        }

    }

    // When clicking hitbox
    private void OnMouseDown()
    {
        Debug.Log("Slot: MouseDown");
        // Try to have the mouse pick up this slot's alien
        if (!hidden)
        {
            dragManager.StartDragging(alien, this);

            // Uncomment to immediately turn off the white hover glow when picking up an alien:
            //hover.color = Color.clear;
        }
    }

    // When mouse is released within the hitbox
    private void OnMouseUp()
    {
        Debug.Log("Slot: MouseUp");
        if (!hidden)
        {

            // Try to put the alien in this slot
            if (dragManager.GetCurrentSlot() != null)
            {
                dragManager.TryPlaceDragged(dragManager.GetCurrentSlot());
            }
            else
            {
                dragManager.Drop();
            }

            hover.color = Color.clear;
        }
    }

    // Make the planet unavailable. Probably only used when starting up the scene
    public void Hide()
    {
        hidden = true;
        hide.color = hideOldColor;

    }

    // Show/discover the planet
    public void Discover()
    {
        hidden = false;
        hide.color = Color.clear;
    }

    public string GetTerrain()
    {
        return Terrain;
    }

    public string GetTemp()
    {
        return Temp;
    }

    //public string GetPlanetTerrain()
    //{
    //    return gameObject.GetComponent<Planet>().GetTerrain();
    //}

    //public string GetPlanetTemp()
    //{
    //    return gameObject.GetComponent<Planet>().GetTemp();
    //}

}
