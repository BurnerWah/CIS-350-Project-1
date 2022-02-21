/*
 * Robert Krawczyk
 * Project 2
 * Manages tutorial
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject filter;
    public GameObject[] planets, shipSlots;
    public GameObject progressBar, turnCounter, evilShip;
    SpriteRenderer filterSR;

    bool focusing = false;
    float focus_z = -10f; // When focusing on objects, we put them at the very front so they are on top. This is that position
    List<GameObject> focus_objs;
    List<float> focus_objs_prefocus_z;

    int part = -1; // Current section of the tutorial
    Dictionary<int, List<GameObject>> graphics; // Populated in start()
    string graphicsPrefix = "Tut_";

    public bool IsFocusing() { return focusing; }

    void Start()
    {
        filterSR = filter.GetComponent<SpriteRenderer>();
        filterSR.color = TranslucentColor(0);

        // Get the graphics gameobjects (children of TutorialManager)
        foreach(SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
        {
            // If the child is named "Tut_x..." then add it to the graphics dictionary
            if(child.name.Substring(0,graphicsPrefix.Length) == graphicsPrefix)
            {
                int graphicPart;
                if(int.TryParse(child.name.Substring(graphicsPrefix.Length, graphicsPrefix.Length + 1), out graphicPart))
                {
                    graphics[graphicPart].Add(child.gameObject);
                }
            }
        }
        // Get any objects named correctly under the Canvas
        for (int i = 0 ; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            // If the child is named "Tut_x..." then add it to the graphics dictionary
            if (child.name.Substring(0, graphicsPrefix.Length) == graphicsPrefix)
            {
                int graphicPart;
                if (int.TryParse(child.name.Substring(graphicsPrefix.Length, graphicsPrefix.Length + 1), out graphicPart))
                {
                    graphics[graphicPart].Add(child);
                }
            }
        }

        // Assigning planets to tutorial parts
        foreach(GameObject shipSlot in shipSlots)
        {
            graphics[0].Add(shipSlot);
        }
        graphics[1].Add(planets[2]);
        graphics[2].Add(planets[2]);
        graphics[2].Add(planets[4]);
        graphics[2].Add(progressBar);
        graphics[3].Add(progressBar);
        graphics[4].Add(turnCounter);
        graphics[5].Add(evilShip);

        // Hide all planets to start
        foreach(GameObject planet in planets)
        {
            planet.GetComponent<Slot>().Hide();
        }

        // Tutorial Part 0: Highlighting ship slots
        StartFocusingOn(graphics[0]);
        part = 0;
    }

    void Update()
    {
        switch (part)
        {
            case 0:
                // Tutorial Part 0 Condition: Click
                if (Input.GetMouseButtonDown(0))
                {
                    part = 1;
                    StopFocusing();
                    StartFocusingOn(graphics[1]);
                    planets[2].GetComponent<Slot>().Discover();
                }
                break;
            case 1:
                // Tutorial Part 1: Move any alien to the earth-like planet

                // If an alien has been moved to planet 2, this part is complete
                if(planets[2].GetComponent<Slot>().alien != null)
                {
                    StopFocusing();
                    StartFocusingOn(graphics[2]);
                    planets[4].GetComponent<Slot>().Discover();
                }
                break;
            case 2:
                // Tutorial Part 1: Move any alien to the other planet
                if (planets[4].GetComponent<Slot>().alien != null)
                {
                    StopFocusing();
                    StartFocusingOn(graphics[2]);
                }
                break;
        }
    }

    void StartFocusingOn(List<GameObject> objects)
    {
        focusing = true;

        // Blacken rest of screen
        filterSR.color = TranslucentColor(.8f);

        // Put all objects being focused on to the foreground
        focus_objs_prefocus_z = new List<float>();
        foreach (GameObject obj in objects){
            focus_objs_prefocus_z.Add(obj.transform.position.z);
            obj.transform.position = SetZ(obj.transform.position, focus_z);
        }
        
    }

    void StopFocusing()
    {
        focusing = false;

        // Blacken rest of screen
        filterSR.color = TranslucentColor(0);

        // Put objects back to where they were
        for (int i = 0; i < focus_objs.Count; i++)
        {
            focus_objs[i].transform.position = SetZ(focus_objs[i].transform.position, focus_objs_prefocus_z[i]);
        }
    }

    Vector3 SetZ(Vector3 vec, float z)
    {
        return new Vector3(vec.x, vec.y, z);
    }

    Color TranslucentColor(float alpha)
    {
        return new Color(1, 1, 1, alpha);
    }
}
