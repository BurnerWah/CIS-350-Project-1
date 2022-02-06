using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    // References
    private SpriteManager spriteManager;

    /* The resources on this planet. Options:
     * Water
     * Oxygen
     * Mineral
     * High temperature
     * Medium temperature?
     * Low temperature
     * I forget the others
     */
    public string[] resources;
    private List<Sprite> resources_sprites = new List<Sprite>();

    // Private variables for handling resource display
    float resourceSprite_width = .4f;
    float popup_width = 1.5f;

    // Private variables for handling the animation
    bool goingUp, stayingUp, goingDown, stayingDown = false;
    float goUp_duration = .3f;
    float startedGoingUp;
    float goDown_duration = .1f;
    float startedGoingDown;
    float upPosY;
    float updownDistance = 1;

    // Start is called before the first frame update
    void Start()
    {
        spriteManager = Object.FindObjectOfType<SpriteManager>();

        upPosY = transform.position.y;
        transform.position += Vector3.down * updownDistance;
        GetComponent<SpriteRenderer>().enabled = false;

        UpdateResourceDisplay();
    }
    public void GoUp()
    {
        if (!stayingUp)
        {
            goingDown = stayingUp = stayingDown = false;
            goingUp = true;
            startedGoingUp = Time.time;
            GetComponent<SpriteRenderer>().enabled = true;
        }
        
    }

    public void GoDown()
    {
        if (stayingUp)
        {
            goingDown = true;
            startedGoingDown = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            if(Time.time < startedGoingUp + goUp_duration)
            {
                // I don't expect this to work because math never seems to work for me, but it could be cool.
                // (Alternatively, we could just learn how to do animations and then just activate the animation instead of doing this)
                // y = cos(pi*x) + b, but fit to the first half of a cosine graph to get an ease-in,ease-out effect
                // x is the percentage of the time duration we are now at
                float newY = Mathf.Cos((Mathf.PI / goUp_duration) * (goUp_duration - (Time.time - startedGoingUp))) + (upPosY - updownDistance);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else
            {
                goingUp = false;
                stayingUp = true;
            }
        }
        else if (stayingUp)
        {
            // Should be unnecessary to force it to stick at the top, but might as well do it
            transform.position = transform.position = new Vector3(transform.position.x, transform.position.y, upPosY);
        }
        else if(goingDown)
        {
            if (Time.time < startedGoingDown + goDown_duration)
            {
                print("going down");
                // Same thing but adding a pi so that we get the second half of a cosine graph
                // y = cos(pi*x+pi) + b
                float newY = Mathf.Cos(Mathf.PI + (Mathf.PI / goDown_duration) * (goDown_duration - (Time.time - startedGoingDown))) + (upPosY - updownDistance);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else
            {
                goingDown = false;
                stayingDown = true;
            }
        }
        else if (stayingDown)
        {
            // Should be unnecessary to force it to stick at the bottom, but might as well do it
            transform.position = transform.position = new Vector3(transform.position.x, transform.position.y, upPosY-updownDistance);
        }
    }

    void UpdateResourceDisplay()
    {
        resources_sprites.Clear();
        foreach(string resource in resources){
            switch (resource.ToLower())
            {
                case "water":
                    resources_sprites.Add(spriteManager.water);
                    break;
                // TODO add the others
                default:
                    print(name + ": " + resource + " is not a valid resource name");
                    break;
            }
        }
        // TODO Spawn SpriteRenderers
        // TODO Change their positions to be lined up and centered within the popup
    }
}
