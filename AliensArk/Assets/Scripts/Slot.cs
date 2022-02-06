/*
 * Robert Krawczyk, Gerard Lamoureux
 * Project1
 * Controls hovering and hiding graphics, and keeps a reference to this slot's alien
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool DEBUG = true;

    // Object references
    Popup popup;
    DragManager dragManager;
    SpriteRenderer spriteRenderer;
    GameObject hover;
    SpriteRenderer hover_spriteRenderer; // the white glow on hover
    GameObject hide;
    SpriteRenderer hide_spriteRenderer; // the black layer when a planet has not been discovered

    // Public variables
    public Alien alien; // can only hold one
    public bool hidden; // Check this in the Unity scene for this slot to start hidden

    // Private variables
    float hoverAlpha_dragging;
    float hoverAlpha_normal;
    float hideAlpha;
    

    // Start is called before the first frame update
    void Start()
    {
        // Search Scene
        dragManager = Object.FindObjectOfType<DragManager>();

        popup = GetComponentInChildren<Popup>(); // Gets the first popup child

        // Start up the hovering glow object
        hover = gameObject.transform.GetChild(0).gameObject; // First child should be the hover object. Ship slots are never hidden so they will have a placeholder instead
        hover_spriteRenderer = hover.GetComponent<SpriteRenderer>();
        hoverAlpha_dragging = hover_spriteRenderer.color.a;
        hoverAlpha_normal = hoverAlpha_dragging / 2;
        hover_spriteRenderer.color = new Color(1, 1, 1, 0); // White (normal) with 0 opacity

        // Start up the hiding object
        hide = gameObject.transform.GetChild(1).gameObject; // Second child should be the hide object
        hide_spriteRenderer = hide.GetComponent<SpriteRenderer>();
        hideAlpha = hide_spriteRenderer.color.a;
        hide_spriteRenderer.color = new Color(1, 1, 1, 0); // White (normal) with 0 opacity

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
        if(alien != null)
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
            if(popup != null)
            {
                popup.GoUp();
            }

            // Display the white glow
            if (dragManager.dragging)
            {
                // (I think 1 is the max for a color's RGBA channel, not 255 like you might think)
                hover_spriteRenderer.color = new Color(1, 1, 1, hoverAlpha_dragging); // medium opacity
            }
            else
            {
                hover_spriteRenderer.color = new Color(1, 1, 1, hoverAlpha_normal); // low opacity
            }
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
            hover_spriteRenderer.color = new Color(1, 1, 1, 0); // White with 0 opacity
        }
            
    }

    // When clicking hitbox
    private void OnMouseDown()
    {
        if (DEBUG)
            print("Slot: MouseDown");
        // Try to have the mouse pick up this slot's alien
        if (!hidden)
        {
            dragManager.StartDragging(alien, this);

            // Uncomment to immediately turn off the white hover glow when picking up an alien:
            //hover_spriteRenderer.color = new Color(1, 1, 1, 0); // 0 opacity
        }
    }

    // When mouse is released within the hitbox
    private void OnMouseUp()
    {
        if (DEBUG)
            print("Slot: MouseUp");
        if (!hidden)
        {

            // Try to put the alien in this slot
            if (dragManager.GetCurrentSlot() != null)
                dragManager.TryPlaceDragged(dragManager.GetCurrentSlot());
            else
                dragManager.Drop();
            hover_spriteRenderer.color = new Color(1, 1, 1, 0); // 0 opacity
        }
    }

    // Make the planet unavailable. Probably only used when starting up the scene
    public void Hide()
    {
        hidden = true;
        hide_spriteRenderer.color = new Color(1, 1, 1, hideAlpha); // high opacity
        
    }

    // Show/discover the planet
    public void Discover()
    {
        hidden = false;
        hide_spriteRenderer.color = new Color(1, 1, 1, 0); // 0 opacity
    }
}
