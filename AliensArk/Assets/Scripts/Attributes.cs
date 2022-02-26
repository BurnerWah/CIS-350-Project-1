/*
 * Gerard Lamoureux
 * Project1
 * Handles different attributes
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    //attributes defined in Unity Editor for easy editing.
    public string[] terrain;
    public string[] atmo;
    public string[] resources;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //will return a random attribute from the array of attributes. Can be used by Aliens and Planets.
    public string GetRandomTerrain() => terrain[Random.Range(0, terrain.Length)];
    public string GetRandomAtmo() => atmo[Random.Range(0, atmo.Length)];
    public string GetRandomResource() => resources[Random.Range(0, resources.Length)];
}