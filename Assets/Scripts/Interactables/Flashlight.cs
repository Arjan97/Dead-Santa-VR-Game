// Script: Flashlight
// Description: Controls the activation and deactivation of a flashlight using XR interactions.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flashlight : MonoBehaviour
{
    // Variable to track the state of the flashlight (on/off)
    private bool lightOn = false;

    void Start()
    {
        // Subscribe to the activated event of the XRGrabInteractable
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(ToggleLights);
    }

    // Method to toggle the state of the flashlight
    private void ToggleLights(ActivateEventArgs args)
    {
        // Invert the state of the light
        lightOn = !lightOn;

        // Get the first child of the flashlight (assumed to be the light source)
        Transform firstChild = transform.GetChild(0);

        // Check if the first child exists
        if (firstChild != null)
        {
            // Activate or deactivate the light based on the lightOn state
            firstChild.gameObject.SetActive(lightOn);
        }
    }
}
