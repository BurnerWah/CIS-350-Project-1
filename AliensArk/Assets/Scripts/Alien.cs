/*
 * Robert Krawczyk, Gerard Lamoureux, Jaden Pleasants, Conner Ogle
 * Project1
 * Just knows which slot it's in, creates random name
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Alien : MonoBehaviour
{
    // This script so far only covers dragging and dropping the alien.
    // You could check 'slot' for what resources the planet/ark slot has, to make this alien die or something :)
    private AttributeStorage.Terrain terrain;
    public AttributeStorage.Terrain Terrain => terrain;

    private AttributeStorage.Temperature temperature;
    public AttributeStorage.Temperature Temperature => temperature;

    private AttributeStorage.Resource resource;
    public AttributeStorage.Resource Resource => resource;

    private AttributeStorage.Atmosphere atmosphere;
    public AttributeStorage.Atmosphere Atmosphere => atmosphere;

    private int health;
    public int Health => health;

    public static readonly int MAX_HAPPINESS = 5;
    private int happiness;
    public int Happiness => happiness;

    private Slot slot;
    public Slot Slot
    {
        get => slot; set
        {
            slot = value;
            UpdateHappiness(); // small assurance that happiness is updated
        }
    }

    //Just a QOL thing, Give each species a randomly generated name.
    private string species;
    public string SpeciesName => species;

    public List<Sprite> AlienSprites;

    public SpriteRenderer spriteRenderer;

    private TurnManager TM;

    // Start is called before the first frame update
    void Start()
    {
        species = MakeRandomName();
        // Register health updates with the turn update event.
        TM = TurnManager.GetTurnManager();
        TM.TurnEvent.AddListener(UpdateHealth);
        // Get random attributes
        terrain = AttributeStorage.Shuffle<AttributeStorage.Terrain>(AttributeStorage.PlanetTerrains).First();
        temperature = AttributeStorage.Shuffle<AttributeStorage.Temperature>(AttributeStorage.PlanetTemps).First();
        resource = AttributeStorage.Shuffle<AttributeStorage.Resource>(AttributeStorage.Resources).First();
        atmosphere = AttributeStorage.Shuffle<AttributeStorage.Atmosphere>(AttributeStorage.Atmospheres).First();
        health = 5;
        spriteRenderer.sprite = AlienSprites[Random.Range(0, AlienSprites.Count)];
        UpdateHappiness();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Updates the happiness of the alien.
    /// </summary>
    /// When the alien on its preffered terrain, happiness is increased by 2;
    /// When the alien on its preffered temperature, happiness is increased by 2;
    /// TODO: When the alien on its preffered atmosphere, happiness is increased by 2;
    /// When the alien detects itself as being on the ship, its minimum happiness should be 3.
    void UpdateHappiness()
    {
        int newHappiness = 0;
        if (Slot?.Terrain == Terrain)
        {
            newHappiness += 2;
        }
        else if (Slot?.Terrain == AttributeStorage.Terrain.Ship)
        {
            newHappiness += 1;
        }
        if (Slot?.Temp == Temperature)
        {
            newHappiness += 2;
        }
        else if (Slot?.Temp == AttributeStorage.Temperature.Ship)
        {
            newHappiness += 2; // change this back to 1 later
        }
        // Because atmospheres aren't implemented yet, we'll just add 1 to happiness no matter what.
        // newHappiness += 1;
        happiness = Mathf.Clamp(newHappiness, 0, MAX_HAPPINESS);
    }

    void UpdateHealth()
    {
        UpdateHappiness();
        Debug.Log($"{species} happiness: {happiness}");
        if (happiness < 3)
        {
            health--;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        GameObject.Find("/WinManager").GetComponent<WinLossManager>().aliensDestroyed++;
        TM.TurnEvent.RemoveListener(UpdateHealth);
    }

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
