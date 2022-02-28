using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    // References
    public SpriteManager spriteManager;
    private SpriteRenderer spriteRenderer;

    /* The resources on this planet. Options:
     * Oxygen/Nitrogen/NoAir
     * Soil/Ocean/Rocky
     * Hot/Warm/Cold
     * Iron/Lumber/NoResources
     */
    public string resources; // You type in the resources in the editor, in order, comma separated. ex. "oxygen,soil,warm,lumber"

    // Private variables for handling resource display
    float icon_z = -.5f;
    float icon_width = .4f;
    float popup_margin = 1f;
    float popup_width;

    // Private variables for handling the animation
    float zPos;

    enum State
    {
        GOING_UP, STAYING_UP, GOING_DOWN, STAYING_DOWN
    }
    State currentState;

    float goUp_duration = .3f;
    float startedGoingUp;
    float goDown_duration = .15f;
    float startedGoingDown;
    float upPosY;
    float updownDistance = 1;

    // Start is called before the first frame update
    void Start()
    {
        spriteManager = Object.FindObjectOfType<SpriteManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        zPos = transform.position.z;
        upPosY = transform.position.y;
        transform.position += Vector3.down * updownDistance;
        spriteRenderer.enabled = false;

        popup_width = spriteRenderer.bounds.size.x - popup_margin;
        UpdateResourceDisplay();

        currentState = State.STAYING_DOWN;
    }
    public void GoUp()
    {
        if (currentState != State.STAYING_UP)
        {
            currentState = State.GOING_UP;
            startedGoingUp = Time.time;
            spriteRenderer.enabled = true;
        }
    }

    public void GoDown()
    {
        if (currentState == State.STAYING_UP)
        {
            startedGoingDown = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.GOING_UP:
                {
                    if (Time.time <= startedGoingUp + goUp_duration)
                    {
                        // y = cos(pi*x) + b, but fit to the first half of a cosine graph to get an ease-in,ease-out effect
                        // x is the percentage of the time duration we are now at
                        float newY = Mathf.Cos(Mathf.PI / goUp_duration * (goUp_duration - (Time.time - startedGoingUp))) + (upPosY - updownDistance);
                        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
                    }
                    else
                    {
                        currentState = State.STAYING_UP;
                    }

                    break;
                }

            case State.STAYING_UP:
                // Should be unnecessary to force it to stick at the top, but might as well do it
                transform.position = new Vector3(transform.position.x, transform.position.y, upPosY);
                break;
            case State.GOING_DOWN:
                {
                    if (Time.time <= startedGoingDown + goDown_duration)
                    {
                        print("going down");
                        // Same thing but adding a pi so that we get the second half of a cosine graph
                        // y = cos(pi*x+pi) + b
                        float newY = Mathf.Cos(Mathf.PI + Mathf.PI / goDown_duration * (goDown_duration - (Time.time - startedGoingDown))) + (upPosY - updownDistance);
                        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
                    }
                    else
                    {
                        currentState = State.STAYING_DOWN;
                    }

                    break;
                }

            case State.STAYING_DOWN:
                // Should be unnecessary to force it to stick at the bottom, but might as well do it
                transform.position = new Vector3(transform.position.x, transform.position.y, upPosY - updownDistance);
                break;
        }
    }


    void UpdateResourceDisplay()
    {
        // Spawn the appropriate icons as children of this Popup
        foreach (string resource in resources.Split(','))
        {
            //print($"{transform.parent.name}: Trying to get {resource} icon...");
            ResourceIcon icon;
            if (spriteManager.Icons.TryGetValue(resource, out icon))
            {
                //print($"{transform.parent.name}: Creating icon: {icon.name}");
                Instantiate(icon, transform);
            }
        }
        // Position the icons
        float curr_x = 0 - (popup_width / 2) + (icon_width / 2);
        foreach (ResourceIcon icon in GetComponentsInChildren<ResourceIcon>())
        {
            icon.transform.localPosition = new Vector3(curr_x, 0, icon_z);
            curr_x += icon_width;
        }
    }
}
