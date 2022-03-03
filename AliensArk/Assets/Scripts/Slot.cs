/*
 * Robert Krawczyk, Gerard Lamoureux, Jaden Pleasants Conner Ogle
 * Project1
 * Controls hovering and hiding graphics, and keeps a reference to this slot's alien
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    // Object references
    Popup popup;
    DragManager dragManager;
    TurnManager turnManager;
    SpriteRenderer hover; // the white glow on hover
    SpriteRenderer hide; // the black layer when a planet has not been discovered
    SpriteRenderer locker; // the green? layer when locked
    Text txt_lockedTurnsLeft;

    // Public variables
    public Alien alien; // can only hold one
    public bool isPlanet; // Checked in planet prefab
    public bool hiddenAtStart;
    bool hidden;
    public bool IsHidden => hidden;

    bool locked; // access with IsLocked()
    int turnLocked, lockTime = 5;
    public string SlotTerrain;
    private AttributeStorage.Terrain _terrain;
    public AttributeStorage.Terrain Terrain => _terrain;

    public string SlotTemp;
    private AttributeStorage.Temperature _temp;
    public AttributeStorage.Temperature Temp => _temp;

    // Private variables
    private Color hoverDragColor;
    private Color hoverNormalColor;
    private Color hideOldColor;

    private FeedbackUI feedbackUI;

    // Start is called before the first frame update
    void Start()
    {
        // Search Scene
        dragManager = Object.FindObjectOfType<DragManager>();
        turnManager = TurnManager.GetTurnManager();
        turnManager.TurnEvent.AddListener(NextTurn);

        _terrain = AttributeStorage.ParseTerrain(SlotTerrain);
        _temp = AttributeStorage.ParseTemperature(SlotTemp);

        if (isPlanet)
        {
            _terrain = AttributeStorage.Shuffle<AttributeStorage.Terrain>(AttributeStorage.PlanetTerrains).First();
            _temp = AttributeStorage.Shuffle<AttributeStorage.Temperature>(AttributeStorage.PlanetTemps).First();
            // Popup
            popup = GetComponentInChildren<Popup>();
            // Locking
            txt_lockedTurnsLeft = GetComponentInChildren<Text>();
            txt_lockedTurnsLeft.text = "";
            locked = false;
        }

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

        feedbackUI = GameObject.Find("/FeedbackUIManager").GetComponent<FeedbackUI>();

        // Start hidden or not
        if (hiddenAtStart)
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
        if (alien != null && dragManager.draggedAlien != alien)
        {
            alien.transform.position = new Vector3(transform.position.x, transform.position.y, -1); // Always snap my alien to me, just in case
        }
    }

    // When mouse enters the hitbox
    public void OnMouseEnter()
    {
        if (alien != null)
        {
            feedbackUI.UpdateFeedbackDisplay(alien);
        }
        if (!hidden)
        {
            // Display the resource popup
            popup?.GoUp();

            // Display the white glow
            hover.color = dragManager.dragging ? hoverDragColor : hoverNormalColor;
            dragManager.CurrentSlot = this;
        }

    }

    // When mouse exits the hitbox
    public void OnMouseExit()
    {
        if (alien != null)
        {
            feedbackUI.ResetFeedbackDisplay();
        }
        if (!hidden)
        {
            // Hide the resource popup
            popup?.GoDown();

            // Hide the white glow
            dragManager.CurrentSlot = null;
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
            if (dragManager.CurrentSlot != null)
            {
                dragManager.TryPlaceDragged(dragManager.CurrentSlot);
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
        if (!hidden && isPlanet)
        {
            hidden = true;
            hide.color = hideOldColor;
        }
    }

    // Show/discover the planet
    public void Discover()
    {
        if (hidden && isPlanet)
        {
            hidden = false;
            hide.color = Color.clear;
        }
    }

    public void Lock()
    {
        if (!locked && isPlanet)
        {
            locked = true;
            print($"{name} locked");
            locker.enabled = true;
            turnLocked = turnManager.currentTurn;
        }
    }

    void NextTurn()
    {
        // Decrement locker
        if (locked && isPlanet)
        {
            int lockedTurnsLeft = lockTime - (turnManager.currentTurn - turnLocked);
            if (lockedTurnsLeft > 0)
            {
                txt_lockedTurnsLeft.text = $"{lockedTurnsLeft}";
                // TODO play locked countdown animation (not created yet)
            }
            else
            {
                Unlock();
            }
        }
    }

    private void Unlock() // Could potentially be public
    {
        if (locked && isPlanet)
        {
            locked = false;
            txt_lockedTurnsLeft.text = "";
        }
    }
}
