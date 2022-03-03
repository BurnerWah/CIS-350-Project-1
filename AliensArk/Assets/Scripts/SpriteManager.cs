/*
 * Jaden Pleasants, Gerard Lamoureux
 * Project 2
 * Defines Sprites for Random Planet Icons
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    /* Options:
     * Oxygen/Nitrogen/NoAir
     * Soil/Ocean/Rocky
     * Hot/Warm/Cold
     * Iron/Lumber/NoResource
     */
    // Plug the sprites into these fields
    public Sprite Oxygen, Nitrogen, NoAir, Soil, Ocean, Rocky, Hot, Warm, Cold, Iron, Lumber, NoResource;
    public Dictionary<string, Sprite> Icons;
    // Maybe there could be a similar set of Sprites stored here for planets

    // Awake is called before Start
    void Awake()
    {
        Icons = new Dictionary<string, Sprite> {
            {"oxygen", Oxygen},
            {"nitrogen", Nitrogen},
            {"noair", NoAir},
            {"soil", Soil},
            {"ocean", Ocean},
            {"rocky", Rocky},
            {"hot", Hot},
            {"warm", Warm},
            {"cold", Cold},
            {"iron", Iron},
            {"lumber", Lumber},
            {"noresource", NoResource}
         };
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
