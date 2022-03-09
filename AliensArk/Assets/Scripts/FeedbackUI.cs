/*
 * Jaden Pleasants, Gerard Lamoureux, Robert Krawczyk
 * Project 2
 * Updates feedback data in the UI.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackUI : MonoBehaviour
{
    public Sprite alienHappy;
    public Sprite alienNotHappy;
    public List<Image> alienHappinessIcons;
    [SerializeField] Image terrainIcon, temperatureIcon, atmosphereIcon, resourceIcon;
    // These are the sprite that will be plugged into the above Icon's
    [SerializeField] Sprite atmo_n, atmo_o, atmo_noair, res_iron, res_lum, res_na, temp_cold, temp_hot, temp_warm, ter_desert, ter_soil, ter_water;
    public Dictionary<AttributeStorage.Atmosphere, Sprite> atmo;
    public Dictionary<AttributeStorage.Resource, Sprite> res;
    public Dictionary<AttributeStorage.Temperature, Sprite> temp;
    public Dictionary<AttributeStorage.Terrain, Sprite> ter;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize sprite dictionary
        atmo = new Dictionary<AttributeStorage.Atmosphere, Sprite>() {
            { AttributeStorage.Atmosphere.Oxygen, atmo_o},
            { AttributeStorage.Atmosphere.Nitrogen, atmo_n },
            { AttributeStorage.Atmosphere.None, atmo_noair}
        };
        res = new Dictionary<AttributeStorage.Resource, Sprite>() {
            { AttributeStorage.Resource.Iron, res_iron},
            { AttributeStorage.Resource.Lumber, res_lum },
            { AttributeStorage.Resource.None, res_na}
        };
        temp = new Dictionary<AttributeStorage.Temperature, Sprite>() {
            { AttributeStorage.Temperature.Cold, temp_cold},
            { AttributeStorage.Temperature.Hot, temp_hot },
            { AttributeStorage.Temperature.Warm, temp_warm}
        };
        ter = new Dictionary<AttributeStorage.Terrain, Sprite>() {
            { AttributeStorage.Terrain.Rocky, ter_desert},
            { AttributeStorage.Terrain.Soil, ter_soil },
            { AttributeStorage.Terrain.Ocean, ter_water}
        };

        ResetFeedbackDisplay();
    }

    public void UpdateFeedbackDisplay(Alien alien)
    {
        Debug.Log($"Updating feedback for {alien.name} with happiness: {alien.Happiness}");
        // Update the happiness icons
        for (int i = 0; i < alien.Happiness; i++)
        {
            alienHappinessIcons[i].sprite = alienHappy;
        }
        // Version 1: showing colon text (ex. "Terrain: Rocky)
        //GameObject.Find("/Canvas/AlienFeedback/SpeciesName").GetComponent<Text>().text = $"Name: {alien.SpeciesName} \nHealth: {alien.Health}\n\tTerrain: {alien.Terrain}\n\tTemperature: {alien.Temperature}\n\tAtmosphere: {alien.Atmosphere}\n\tResource: {alien.Resource}";
        // Version 2: not showing colon text (ex. "Rocky")
        GameObject.Find("/Canvas/AlienFeedback/SpeciesName").GetComponent<Text>().text = $"{alien.SpeciesName} has {alien.Health} health\n{alien.SpeciesName} needs:\n\t{alien.Terrain}\n\t{alien.Temperature}\n\t{alien.Atmosphere}\n\t{alien.Resource}";

        GameObject.Find("/Canvas/AlienFeedback/SpeciesName").SetActive(true);

        // Update resource icons
        print(res[alien.Resource]);
        terrainIcon.sprite = ter[alien.Terrain];
        temperatureIcon.sprite = temp[alien.Temperature];
        atmosphereIcon.sprite = atmo[alien.Atmosphere];
        resourceIcon.sprite = res[alien.Resource];
        terrainIcon.gameObject.SetActive(true);
        temperatureIcon.gameObject.SetActive(true);
        atmosphereIcon.gameObject.SetActive(true);
        resourceIcon.gameObject.SetActive(true);
    }

    public void ResetFeedbackDisplay()
    {
        alienHappinessIcons.ForEach(icon => icon.sprite = alienNotHappy);
        GameObject.Find("/Canvas/AlienFeedback/SpeciesName").GetComponent<Text>().text = "No alien selected";
        terrainIcon.gameObject.SetActive(false);
        temperatureIcon.gameObject.SetActive(false);
        atmosphereIcon.gameObject.SetActive(false);
        resourceIcon.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
