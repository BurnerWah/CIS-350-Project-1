/*
 * Robert Krawczyk, Gerard Lamoureux 
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

    public Slot slot;

    //Just a QOL thing, Give each species a randomly generated name.
    private string species;
    

    // Start is called before the first frame update
    void Start()
    {
        species = MakeRandomName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetSpeciesName()
    {
        return species;
    }

    //Generate a random name.
    string MakeRandomName()
    {
        string name = "";
        for(int i=0; i<Random.Range(3,10);i++)
        {
            if (i % 2 == 0)
                name += RandomConsonant(i);
            else
                name += RandomVowel(i);
        }
        return name;
    }
    string RandomConsonant(int i)
    {
        string[] Consonants = new string[21] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
        if(i == 0)
            return Consonants[Random.Range(0, Consonants.Length)].ToUpper();
        else
            return Consonants[Random.Range(0, Consonants.Length)];
    }
    string RandomVowel(int i)
    {
        string[] Vowels = new string[5] { "a", "e", "i", "o", "u" };
        if (i == 0)
            return Vowels[Random.Range(0, Vowels.Length)].ToUpper();
        else
            return Vowels[Random.Range(0, Vowels.Length)];
    }
}
