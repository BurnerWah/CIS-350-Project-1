using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject filter, arrow;
    public Text txt_tutorial1, txt_tuorial2;
    SpriteRenderer filterSR;

    bool focusing = false;
    float focus_z = -10f; // When focusing on objects, we put them at the very front so they are on top. This is that position
    List<GameObject> focus_objs;
    List<float> focus_objs_prefocus_z;

    int part = 0; // Current section of the tutorial

    public bool IsFocusing() { return focusing; }

    void Start()
    {
        filterSR = filter.GetComponent<SpriteRenderer>();
        filterSR.color = TranslucentColor(0);

        //TODO start part 1
        part = 1;
    }

    void Update()
    {
        switch (part)
        {
            case 1:
                //TODO ex. Check if alien has been moved to the planet
                break;
        }
    }

    void StartFocusingOn(GameObject[] objects)
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
