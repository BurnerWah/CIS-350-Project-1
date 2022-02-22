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
    public ResourceIcon Oxygen, Nitrogen, NoAir, Soil, Ocean, Rocky, Hot, Warm, Cold, Iron, Lumber, NoResource;
    public Dictionary<string, ResourceIcon> Icons = new Dictionary<string, ResourceIcon>();
    // Maybe there could be a similar set of Sprites stored here for planets

    // Awake is called before Start
    void Awake()
    {
        Icons.Add("oxygen", Oxygen);
        Icons.Add("nitrogen", Nitrogen);
        Icons.Add("noair", NoAir);
        Icons.Add("soil", Soil);
        Icons.Add("ocean", Ocean);
        Icons.Add("rocky", Rocky);
        Icons.Add("hot", Hot);
        Icons.Add("warm", Warm);
        Icons.Add("cold", Cold);
        Icons.Add("iron", Iron);
        Icons.Add("lumber", Lumber);
        Icons.Add("noresource", NoResource);
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
