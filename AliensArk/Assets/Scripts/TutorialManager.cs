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

    bool focusing = false;
    public bool IsFocusing => focusing;

    float focus_z = -10f; // When focusing on objects, we put them at the very front so they are on top. This is that position
    List<GameObject> focused_graphics;
    List<GameObject> focused_tempGraphics;

    int part = -1; // Current section of the tutorial
    // Dictionary<int, List<GameObject>> _graphics; // Populated in start()
    List<GameObject>[] graphics = new List<GameObject>[10]; // Preexisting objects that are focused on in the tutorial
    List<GameObject>[] tempGraphics = new List<GameObject>[10]; // Tutorial-only objects (like glows) that are turned on or off in the tutorial
    readonly string tempGraphicsPrefix = "Tut_";
    Regex tempGraphicsReg = new Regex(@"^Tut_(\d)", RegexOptions.Compiled);

    void Start()
    {
        filterSR = filter.GetComponent<SpriteRenderer>();
        filterSR.color = TranslucentColor(0);

        // Get any tutorial-only graphics under TutorialManager
        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
        {
            if (tempGraphicsReg.IsMatch(child.name))
            {
                tempGraphics[int.Parse(tempGraphicsReg.Match(child.name).Groups[0].Value)].Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
            // If the child is named "Tut_x..." then add it to the graphics dictionary
            // if (child.name.Substring(0, graphicsPrefix.Length) == graphicsPrefix)
            // {
            //     int graphicPart;
            //     if (int.TryParse(child.name.Substring(graphicsPrefix.Length, graphicsPrefix.Length + 1), out graphicPart))
            //     {
            //         graphics[graphicPart].Add(child.gameObject);
            //     }
            // }
        }
        // Get any objects named correctly under the Canvas
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (tempGraphicsReg.IsMatch(child.name))
            {
                graphics[int.Parse(tempGraphicsReg.Match(child.name).Groups[0].Value)].Add(child);
            }
            // If the child is named "Tut_x..." then add it to the graphics dictionary
            // if (child.name.Substring(0, graphicsPrefix.Length) == graphicsPrefix)
            // {
            //     int graphicPart;
            //     if (int.TryParse(child.name.Substring(graphicsPrefix.Length, graphicsPrefix.Length + 1), out graphicPart))
            //     {
            //         graphics[graphicPart].Add(child);
            //     }
            // }
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
        graphics[1].Add(startingAlien.gameObject);
        // Tutorial Part 2: That's not a good fit for the alien. Move the alien to the other highlighted planet
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

        // Hide all planets to start
        foreach (GameObject planet in planets)
        {
            planet.GetComponent<Slot>().Hide();
        }

        // Tutorial Part 0: Highlighting ship slots
        StartFocusingOn(0);
        part = 0;
    }

    void Update()
    {
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
                }
                break;
            case 1:
                // Tutorial Part 1: Move the starting alien to the earth-like planet

                // If an alien has been moved to planet 2, this part is complete
                // Move the alien to continue
                if (planets[2].GetComponent<Slot>().alien == startingAlien)
                {
                    StopFocusing();
                    StartFocusingOn(2);
                    planets[4].GetComponent<Slot>().Discover();
                    part = 2;
                }
                break;
            case 2:
                // Tutorial Part 2: That's not a good fit for the alien. Move the alien to the other highlighted planet
                // Move the alien to continue
                if (planets[4].GetComponent<Slot>().alien == startingAlien)
                {
                    StopFocusing();
                    StartFocusingOn(3);
                    part = 3;
                }
                break;
            case 3:
                // Tutorial Part 3: The alien is happy. See on the progress bar that one of the lights is lit up
                // Click to continue
                if (Input.GetMouseButtonDown(0))
                {
                    StopFocusing();
                    StartFocusingOn(4);
                    part = 4;
                }
                break;
            case 4:
                // Tutorial Part 4: See the turn counter
                // Click to continue
                if (Input.GetMouseButtonDown(0))
                {
                    StopFocusing();
                    StartFocusingOn(5);
                    part = 5;
                }
                break;
        }
    }


    void StartFocusingOn(int partNo)
    {
        StartFocusingOn(graphics[partNo], tempGraphics[partNo]);
    }
    void StartFocusingOn(List<GameObject> graphics, List<GameObject> tempGraphics)
    {
        focusing = true;

        // Blacken rest of screen
        filterSR.color = TranslucentColor(.8f);

        // Put all objects being focused on to the foreground

        foreach (GameObject obj in graphics)
        {
            obj.transform.position = AddZ(obj.transform.position, focus_z);
        }

        foreach (GameObject obj in tempGraphics)
        {
            obj.SetActive(true);
            obj.transform.position = AddZ(obj.transform.position, focus_z);
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
