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
    TutorialManager tutorialManager;
    WinLossManager winLossManager;


    // Public variables
    public Alien alien; // can only hold one
    public bool isPlanet; // Checked in planet prefab
    public bool hiddenAtStart;
    bool hidden;
    public bool IsHidden => hidden;

    bool locked;
    public bool IsLocked => locked;
    int turnLocked, lockTime = 5;
    public string SlotTerrain;
    private AttributeStorage.Terrain _terrain;
    public AttributeStorage.Terrain Terrain => _terrain;

    public string SlotTemp;
    private AttributeStorage.Temperature _temp;
    public AttributeStorage.Temperature Temp => _temp;

    public string SlotAtmosphere;
    private AttributeStorage.Atmosphere _atmosphere;
    public AttributeStorage.Atmosphere Atmosphere => _atmosphere;

    public string SlotResource;
    private AttributeStorage.Resource _resource;
    public AttributeStorage.Resource Resource => _resource;

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
        tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        winLossManager = GameObject.Find("WinManager").GetComponent<WinLossManager>();

        _terrain = AttributeStorage.ParseTerrain(SlotTerrain);
        _temp = AttributeStorage.ParseTemperature(SlotTemp);
        _atmosphere = AttributeStorage.ParseAtmosphere(SlotAtmosphere);
        _resource = AttributeStorage.ParseResource(SlotResource);

        if (isPlanet)
        {
            _terrain = AttributeStorage.Shuffle<AttributeStorage.Terrain>(AttributeStorage.PlanetTerrains).First();
            _temp = AttributeStorage.Shuffle<AttributeStorage.Temperature>(AttributeStorage.PlanetTemps).First();
            _atmosphere = AttributeStorage.Shuffle<AttributeStorage.Atmosphere>(AttributeStorage.Atmospheres).First();
            _resource = AttributeStorage.Shuffle<AttributeStorage.Resource>(AttributeStorage.Resources).First();

            this.transform.Find("Canvas/Terrain").gameObject.GetComponent<ResourceIcon>().UpdateIcon(_terrain.ToString());
            this.transform.Find("Canvas/Temp").gameObject.GetComponent<ResourceIcon>().UpdateIcon(_temp.ToString());
            if(_atmosphere == AttributeStorage.Atmosphere.None)
                this.transform.Find("Canvas/Atmosphere").gameObject.GetComponent<ResourceIcon>().UpdateIcon("noair");
            else
                this.transform.Find("Canvas/Atmosphere").gameObject.GetComponent<ResourceIcon>().UpdateIcon(_atmosphere.ToString());
            if (_resource == AttributeStorage.Resource.None)
                this.transform.Find("Canvas/Resource").gameObject.GetComponent<ResourceIcon>().UpdateIcon("noresource");
            else
                this.transform.Find("Canvas/Resource").gameObject.GetComponent<ResourceIcon>().UpdateIcon(_resource.ToString());
            // Locking
            txt_lockedTurnsLeft = transform.Find("Canvas").Find("LockedTurnsLeft").GetComponent<Text>();
            txt_lockedTurnsLeft.text = "";
            locker = transform.Find("Locker").gameObject.GetComponent<SpriteRenderer>();
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

        Unlock();

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
        if (alien != null && !winLossManager.gameOver)
        {
            feedbackUI.UpdateFeedbackDisplay(alien, this);
        }
        if (!hidden && !locked && tutorialManager.draggingAllowed && !winLossManager.gameOver)
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
        if (true) // always can hide, for corner cases
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
            locker.gameObject.SetActive(true);
            turnLocked = turnManager.currentTurn;
        }
    }

    public void Unlock()
    {
        if (isPlanet)
        {
            locked = false;
            txt_lockedTurnsLeft.text = "";
            locker.gameObject.SetActive(false);
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
}
