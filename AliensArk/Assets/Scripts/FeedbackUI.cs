/*
 * Jaden Pleasants
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

    // Start is called before the first frame update
    void Start()
    {
        // Make sure that all icons default to being empty
        alienHappinessIcons.ForEach(icon => icon.sprite = alienNotHappy);
    }

    public void UpdateFeedbackDisplay(Alien alien)
    {
        Debug.Log($"Updating feedback for {alien.name} with happiness: {alien.Happiness}");
        // Update the happiness icons
        for (int i = 0; i < alien.Happiness; i++)
        {
            alienHappinessIcons[i].sprite = alienHappy;
        }
    }

    public void ResetFeedbackDisplay()
    {
        alienHappinessIcons.ForEach(icon => icon.sprite = alienNotHappy);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
