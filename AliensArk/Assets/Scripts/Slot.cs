using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    // Object references
    DragManager dragManager;
    SpriteRenderer spriteRenderer;
    GameObject hover;
    SpriteRenderer hover_spriteRenderer; // the white glow on hover
    GameObject hide;
    SpriteRenderer hide_spriteRenderer; // the black layer when a planet has not been discovered

    // Public variables
    public Alien alien; // can only hold one
    public bool hidden = false;

    // Private variables
    float hoverAlpha_dragging;
    float hoverAlpha_normal;
    float hideAlpha;
    

    // Start is called before the first frame update
    void Start()
    {
        // Find objects
        dragManager = Object.FindObjectOfType<DragManager>();

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
    }

    // Update is called once per frame
    void Update()
    {
        if(alien != null)
        {
            alien.transform.position = gameObject.transform.position; // just in case
        }
    }

    // When mouse enters the hitbox
    private void OnMouseEnter()
    {
        if (!hidden)
        {
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
        }
            
    }

    // When mouse exits the hitbox
    private void OnMouseExit()
    {
        if (!hidden)
        {
            // Hide the white glow
            hover_spriteRenderer.color = new Color(1, 1, 1, 0); // White with 0 opacity
        }
            
    }

    // When clicking hitbox
    private void OnMouseDown()
    {
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
        if (!hidden)
        {
            // Try to put the alien in this slot
            dragManager.TryPlace(this);
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
