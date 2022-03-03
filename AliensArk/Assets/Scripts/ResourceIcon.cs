/*
 * Gerard Lamoureux
 * Project 2
 * Changes Planet Attribute Sprite
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceIcon : MonoBehaviour
{
    public Slot planet;
    public bool isTerrain, isTemp, isAtmosphere, isResource;
    public void UpdateIcon(string attr)
    {
        GetComponent<Image>().sprite = GameObject.Find("/SpriteManager").GetComponent<SpriteManager>().Icons[attr.ToLower()];
    }
}
