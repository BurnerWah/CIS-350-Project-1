/*
 * Robert Krawczyk, Jaden Pleasants
 * Project1
 * Just detects if aliens are dropped in space
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    DragManager dragManager;

    private void OnMouseUp()
    {
        Debug.Log("Background: MouseUp");
        dragManager.Drop();
    }

    // Start is called before the first frame update
    void Start()
    {
        dragManager = Object.FindObjectOfType<DragManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
