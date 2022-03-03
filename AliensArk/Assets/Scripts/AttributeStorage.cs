/*
 * Jaden Pleasants, Gerard Lamoureux
 * Project 2
 * Storage structure for attribute data
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is a structure meant for comparing and interacting with resource information.
/// It should not be attached to anything, and intentionally does not extend Monoehaviour.
/// </summary>
public class AttributeStorage
{
    /// <summary>
    /// Enumeration of the attribute types that this class provides.
    /// </summary>
    public enum Types
    {
        Atmosphere, Terrain, Temperature, Resource,
    }

    /// <summary>
    /// Contains possible atmospheres
    /// </summary>
    public enum Atmosphere
    {
        None, Oxygen, Nitrogen, Ship,
    }
    public static HashSet<Atmosphere> Atmospheres = new HashSet<Atmosphere> {
        Atmosphere.None,
        Atmosphere.Oxygen,
        Atmosphere.Nitrogen,
    };

    public static AttributeStorage.Atmosphere ParseAtmosphere(string atmosphere)
    {
        switch (atmosphere.ToLower())
        {
            case "none": return Atmosphere.None;
            case "oxygen": return Atmosphere.Oxygen;
            case "nitrogen": return Atmosphere.Nitrogen;
            case "ship": return Atmosphere.Ship;
            default: throw new System.ArgumentException($"Unexpected input: {atmosphere}");
        }
    }

    /// <summary>
    /// Contains possible terrains
    /// </summary>
    public enum Terrain
    {
        Soil, Ocean, Rocky, Ship,
    }
    /// <summary>
    /// Set of all terrains a planet can have
    /// </summary>
    public static HashSet<Terrain> PlanetTerrains = new HashSet<Terrain> {
        Terrain.Soil,
        Terrain.Ocean,
        Terrain.Rocky,
    };
    public static HashSet<Terrain> ShipTerrains = new HashSet<Terrain> { Terrain.Ship };
    public static HashSet<Terrain> AllTerrains => PlanetTerrains.Union(ShipTerrains) as HashSet<Terrain>;
    /// <summary>
    /// Converts a string provided by the unity inspector to a terrain type
    /// </summary>
    /// <param name="terrain">String provided by unity inspector</param>
    /// <returns>A terrain enum value</returns>
    /// <exception cref="System.ArgumentException"></exception>
    public static AttributeStorage.Terrain ParseTerrain(string terrain)
    {
        switch (terrain.ToLower())
        {
            case "soil": return Terrain.Soil;
            case "ocean": return Terrain.Ocean;
            case "rocky": return Terrain.Rocky;
            case "ship": return Terrain.Ship;
            default: throw new System.ArgumentException($"Unexpected input: {terrain}");
        }
    }

    /// <summary>
    /// Contains temperatures
    /// </summary>
    public enum Temperature
    {
        Hot, Warm, Cold, Ship,
    }
    public static HashSet<Temperature> PlanetTemps = new HashSet<Temperature> {
        Temperature.Hot,
        Temperature.Warm,
        Temperature.Cold,
    };
    public static HashSet<Temperature> ShipTemps = new HashSet<Temperature> { Temperature.Ship };
    public static HashSet<Temperature> AllTemps => PlanetTemps.Union(ShipTemps) as HashSet<Temperature>;
    public static AttributeStorage.Temperature ParseTemperature(string temp)
    {
        switch (temp.ToLower())
        {
            case "hot": return Temperature.Hot;
            case "warm": return Temperature.Warm;
            case "cold": return Temperature.Cold;
            case "ship": return Temperature.Ship;
            default: throw new System.ArgumentException($"Unexpected input: {temp}");
        }
    }

    /// <summary>
    /// Contains possible resources.
    /// Unlike other enumerations in AttributeStorage, it's advisable for this to be stored in a collection.
    /// </summary>
    public enum Resource
    {
        None, Iron, Lumber, Ship
    }
    public static HashSet<Resource> Resources = new HashSet<Resource> {
        Resource.None,
        Resource.Iron,
        Resource.Lumber,
    };

    public static AttributeStorage.Resource ParseResource(string resource)
    {
        switch (resource.ToLower())
        {
            case "none": return Resource.None;
            case "iron": return Resource.Iron;
            case "lumber": return Resource.Lumber;
            case "ship": return Resource.Ship;
            default: throw new System.ArgumentException($"Unexpected input: {resource}");
        }
    }

    /// <summary>
    /// Shuffles a collection
    /// </summary>
    /// <typeparam name="T">A collection type (should be one from this class)</typeparam>
    /// <param name="items">The collection to shuffle</param>
    /// <returns>An enumerator for the shuffled collection</returns>
    public static IEnumerable<T> Shuffle<T>(IEnumerable<T> items) => (
        from i in items
        orderby Random.Range(0f, 1f) descending
        select i
    );
}
