/*
 * Robert Krawczyk, Gerard Lamoureux, Jaden Pleasants Conner Ogle
 * Project1
 * Just knows which slot it's in, creates random name
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    // This script so far only covers dragging and dropping the alien.
    // You could check 'slot' for what resources the planet/ark slot has, to make this alien die or something :)
    public string Terrain;
    public string Temp;
    
    public Slot slot;

    //Just a QOL thing, Give each species a randomly generated name.
    private string species;
    private string[] attributes = new string[3];


    // Start is called before the first frame update
    void Start()
    {
        species = MakeRandomName();
        //attributes[0] = GameObject.FindGameObjectWithTag("Attributes").GetComponent<Attributes>().GetRandomTerrain();
        //attributes[1] = GameObject.FindGameObjectWithTag("Attributes").GetComponent<Attributes>().GetRandomAtmo();
        //attributes[2] = GameObject.FindGameObjectWithTag("Attributes").GetComponent<Attributes>().GetRandomResource();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetSpeciesName()
    {
        return species;
    }
    public string GetTerrain()
    {
        return Terrain;
    }

    public string GetTemp()
    {
        return Temp;
    }

    //get the alien attributes
    public string GetAlienTerrain() => attributes[0];
    public string GetAlienAtmo() => attributes[1];
    public string GetAlienResource() => attributes[2];

    //Generate a random name.
    string MakeRandomName()
    {
        string name = "";
        name += RandomConsonant().ToString().ToUpper();
        for (int i = 1; i < Random.Range(3, 10); i++)
        {
            name += (i % 2 == 0) ? RandomConsonant() : RandomVowel();
        }
        return name;
    }
    private static readonly char[] Consonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' };
    private static readonly char[] Vowels = { 'a', 'e', 'i', 'o', 'u' };
    private char RandomConsonant() => Consonants[Random.Range(0, Consonants.Length)];
    private char RandomVowel() => Vowels[Random.Range(0, Vowels.Length)];
}
