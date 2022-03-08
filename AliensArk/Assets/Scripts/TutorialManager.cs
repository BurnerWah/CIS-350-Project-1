/*
 * Robert Krawczyk, Jaden Pleasants
 * Project 2
 * Manages tutorial
 */
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Alien startingAlien;
    public GameObject canvas;
    public GameObject filter;
    public GameObject[] planets, shipSlots;
    public GameObject progressBar, turnCounter, evilShip;
    SpriteRenderer filterSR;
    SpawnManager spawnManager;

    bool focusing = false;
    public bool IsFocusing => focusing;

    float focus_z = 0;//-30f; // When focusing on objects, we put them at the very front so they are on top. This is that position
    List<GameObject> focused_graphics = new List<GameObject>();
    List<GameObject> focused_tempGraphics = new List<GameObject>();

    int part = -1; // Current section of the tutorial
    // Dictionary<int, List<GameObject>> _graphics; // Populated in start()
    List<GameObject>[] graphics = new List<GameObject>[6]; // Preexisting objects that are focused on in the tutorial
    List<GameObject>[] tempGraphics = new List<GameObject>[6]; // Tutorial-only objects (like glows) that are turned on or off in the tutorial
    readonly string tempGraphicsPrefix = "Tut_";
    //Regex tempGraphicsReg = new Regex(@"^Tut_(\d)", RegexOptions.Compiled);

    float curr_clickCooldown = 0, clickCooldown = .2f;
    public bool draggingAllowed = false;

    void Start()
    {
        for(int i = 0; i < 6; i++)
        {
            graphics[i] = new List<GameObject>();
            tempGraphics[i] = new List<GameObject>();
        }

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        filterSR = filter.GetComponent<SpriteRenderer>();
        filterSR.color = TranslucentColor(0);

        // Get any tutorial-only graphics under TutorialManager
        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
        {
            // Commented this during debugging, will uncomment after playtest
            /*
            if (tempGraphicsReg.IsMatch(child.name))
            {
                tempGraphics[int.Parse(tempGraphicsReg.Match(child.name).Groups[0].Value)].Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
            */
            // If the child is named "Tut_x..." then add it to the graphics list
            if (child.name.Substring(0, tempGraphicsPrefix.Length) == tempGraphicsPrefix)
            {
                int graphicPart;
                string graphicPart_str = child.name.Substring(tempGraphicsPrefix.Length, 1);
                if (int.TryParse(graphicPart_str, out graphicPart))
                {
                    tempGraphics[graphicPart].Add(child.gameObject);
                }
            }
        }
        // Get any objects named correctly under the Canvas
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            GameObject child = canvas.transform.GetChild(i).gameObject;
            //print($"checking child {child.name}");
            /*
            if (tempGraphicsReg.IsMatch(child.name))
            {
                graphics[int.Parse(tempGraphicsReg.Match(child.name).Groups[0].Value)].Add(child);
                print("Added tempGraphic");
            }
            */
            // If the child is named "Tut_x..." then add it to the graphics dictionary
            if (child.name.Substring(0, tempGraphicsPrefix.Length) == tempGraphicsPrefix)
            {
                int graphicPart;
                if (int.TryParse(child.name.Substring(tempGraphicsPrefix.Length, 1), out graphicPart))
                {
                    tempGraphics[graphicPart].Add(child);
                }
            }
        }

        // Deciding which pre-existing objects are active during which tutorial part.
        //(Any temporary tutorial-only object can be named "Tut_X" to auto assign it to part X)
        // Tutorial Part 0: Highlighting ship slots
        foreach (GameObject shipSlot in shipSlots)
        {
            graphics[0].Add(shipSlot);
        }
        // Tutorial Part 1: Move the starting alien to the earth-like planet
        graphics[1].Add(startingAlien.gameObject);
        graphics[1].Add(planets[2]);
        // Tutorial Part 2: That's not a good fit for the alien. Move the alien to the other highlighted planet
        graphics[2].Add(startingAlien.gameObject);
        graphics[2].Add(planets[2]);
        graphics[2].Add(planets[4]);
        graphics[2].Add(progressBar);
        // Tutorial Part 3: See progress bar
        graphics[3].Add(progressBar);
        // Tutorial Part 4: There's a turn counter
        graphics[4].Add(turnCounter);
        graphics[4].Add(evilShip);
        // Tutorial Part 5: There's an evil ship
        graphics[5].Add(turnCounter);
        graphics[5].Add(evilShip);

        ///// Start the tutorial

        // Turning off alien spawner temporarily
        spawnManager.enabled = false;

        // Hide all temp graphics to start
        foreach(List<GameObject> tempGraphicList in tempGraphics)
        {
            foreach(GameObject tempGraphic in tempGraphicList)
            {
                tempGraphic.SetActive(false);
            }
        }

        // Hide all planets to start
        foreach (GameObject planet in planets)
        {
            //planet.GetComponent<Slot>().Hide();
        }

        if (Globals.curr_highScore != 0) // Not first time
        {
            Destroy(gameObject);
            return;
        }

        // Tutorial Part 0: Highlighting ship slots
        StartFocusingOn(0);
        part = 0;
    }

    void Update()
    {
        curr_clickCooldown -= Time.deltaTime;

        bool alienMoved = false;
        GameObject prevPlanet = gameObject; // dummy init
        switch (part)
        {
            case 0:
                // Tutorial Part 0: The ship slots are highlighted
                // Click to continue
                if (Input.GetMouseButtonDown(0))
                {
                    StopFocusing();
                    StartFocusingOn(1);
                    planets[2].GetComponent<Slot>().Discover();
                    part = 1;
                    curr_clickCooldown = clickCooldown;
                }
                break;
            case 1:
                // Tutorial Part 1: Move the starting alien
                // Move the alien anywhere to continue
                draggingAllowed = true;
                
                foreach (GameObject planet in planets)
                {
                    if (planet.GetComponent<Slot>().alien == startingAlien){
                        alienMoved = true;
                        prevPlanet = planet;
                        break;
                    }
                }
                if (alienMoved)
                {
                    draggingAllowed = false;
                    StopFocusing();
                    StartFocusingOn(2);
                    planets[4].GetComponent<Slot>().Discover();
                    part = 2; // Skipping part 2 because of new mechanic
                }
                break;
            
            case 2:
                // Tutorial Part 2: You can't move them the turn after you place them
                // Click to continue
                if (Input.GetMouseButtonDown(0) && curr_clickCooldown <= 0)
                {
                    StopFocusing();
                    StartFocusingOn(3);
                    part = 3;
                    curr_clickCooldown = clickCooldown;
                }
                break;
            
            case 3:
                // Tutorial Part 3: The alien is happy. See on the progress bar that one of the lights is lit up
                // Click to continue
                if (Input.GetMouseButtonDown(0) && curr_clickCooldown <= 0)
                {
                    StopFocusing();
                    StartFocusingOn(4);
                    part = 4;
                    curr_clickCooldown = clickCooldown;
                }
                break;
            case 4:
                // Tutorial Part 4: See the turn counter
                // Click to continue
                if (Input.GetMouseButtonDown(0) && curr_clickCooldown <= 0)
                {
                    StopFocusing();
                    StartFocusingOn(5);
                    part = 5;
                    curr_clickCooldown = clickCooldown;
                }
                break;
            case 5:
                // Evil ship
                // Click to continue
                if (Input.GetMouseButtonDown(0) && curr_clickCooldown <= 0)
                {
                    StopFocusing();
                    part = 6;
                    curr_clickCooldown = clickCooldown;
                }
                break;
            case 6: // Done
                draggingAllowed = true;
                spawnManager.enabled = true;
                //TurnManager.GetTurnManager().Reset();
                foreach(GameObject planet in planets)
                {
                    planet.GetComponent<Slot>().Discover();
                }
                break;
        }
    }


    void StartFocusingOn(int partNo)
    {
        StartFocusingOn(graphics[partNo], tempGraphics[partNo]);
    }
    void StartFocusingOn(List<GameObject> f_graphics, List<GameObject> f_tempGraphics)
    {
        focusing = true;

        // Blacken rest of screen
        filterSR.color = TranslucentColor(.6f);

        // Put all objects being focused on to the foreground

        foreach (GameObject obj in f_graphics)
        {
            obj.transform.position = AddZ(obj.transform.position, focus_z);
            focused_graphics.Add(obj);
        }

        foreach (GameObject obj in f_tempGraphics)
        {
            obj.SetActive(true);
            obj.transform.position = AddZ(obj.transform.position, focus_z);
            focused_tempGraphics.Add(obj);
        }
    }

    void StopFocusing()
    {
        focusing = false;

        // Blacken rest of screen
        filterSR.color = TranslucentColor(0);

        // Put graphics and tempGraphics back to where they were
        for (int i = 0; i < focused_graphics.Count; i++)
        {
            focused_graphics[i].transform.position = AddZ(focused_graphics[i].transform.position, -focus_z);
        }
        for (int i = 0; i < focused_tempGraphics.Count; i++)
        {
            focused_tempGraphics[i].SetActive(false);
            focused_tempGraphics[i].transform.position = AddZ(focused_tempGraphics[i].transform.position, -focus_z);
        }

        // Reset storage
        focused_graphics = new List<GameObject>();
        focused_tempGraphics = new List<GameObject>();
    }

    Vector3 AddZ(Vector3 vec, float z)
    {
        return new Vector3(vec.x, vec.y, vec.z + z);
    }

    Color TranslucentColor(float alpha)
    {
        return new Color(1, 1, 1, alpha);
    }
}
