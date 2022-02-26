/*
 * Jaden Pleasants
 * Project 2
 * Manager/tracker for slots
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    // These need to be set in the inspector, but they shouldn't actually see any real use
    public GameObject[] _planetSlots;
    public GameObject[] _shipSlots;

    // Real slot collections
    HashSet<GameObject> planetSlots;
    HashSet<GameObject> shipSlots;

    public HashSet<GameObject> PlanetSlots => planetSlots;
    public HashSet<GameObject> ShipSlots => shipSlots;

    // Pseudo-object for looking at all slots
    // It cannot be written to, as set membership is merely defined as being in PlanetSlots or ShipSlots
    public HashSet<GameObject> AllSlots
    {
        get
        {
            var x = new HashSet<GameObject>(planetSlots);
            x.UnionWith(shipSlots);
            return x;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        planetSlots = new HashSet<GameObject>(_planetSlots);
        shipSlots = new HashSet<GameObject>(_shipSlots);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
