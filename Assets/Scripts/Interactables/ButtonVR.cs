// Script: ButtonVR
// Description: Handles virtual reality button interactions, triggering events on press and release.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    // Reference to the button game object
    public GameObject button;

    // Events triggered on button press and release
    public UnityEvent OnPress;
    public UnityEvent OnRelease;

    // Reference to the object pressing the button
    GameObject presser;

    // Reference to the GunHandler script for handling rapid-fire mode
    public GunHandler gunHandler;

    // Flag indicating whether the button is currently pressed
    private bool isPressed;

    // Flag indicating whether the gun is in rapid-fire mode
    private bool isRapidFireMode = false;

    void Start()
    {
        // Initialize the button state
        isPressed = false;
    }

    // Called when an object enters the trigger zone of the button
    private void OnTriggerEnter(Collider other)
    {
        // Check if the button is not already pressed
        if (!isPressed)
        {
            // Move the button to the pressed position
            button.transform.localPosition = new Vector3(-0.3f, 0, 0);
            // Store the object pressing the button
            presser = other.gameObject;
            // Invoke the OnPress event
            OnPress.Invoke();
            // Update the pressed state
            isPressed = true;
        }
    }

    // Called when an object exits the trigger zone of the button
    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the one that pressed the button
        if (other.gameObject == presser)
        {
            // Move the button to the released position
            button.transform.localPosition = new Vector3(-1.199f, 0, 0);
            // Invoke the OnRelease event
            OnRelease.Invoke();
            // Update the pressed state
            isPressed = false;
        }
    }

    // Method to toggle between rapid-fire and single-fire modes
    public void ToggleFireMode()
    {
        // Invert the rapid-fire mode flag
        isRapidFireMode = !isRapidFireMode;

        // Check if a GunHandler is assigned
        if (gunHandler)
        {
            // Set the rapid-fire mode in the GunHandler
            gunHandler.SetRapidFireMode(isRapidFireMode);

            // Log the mode switch
            Debug.Log("Switched to " + (isRapidFireMode ? "Rapid-Fire" : "Single-Fire") + " mode.");
        }
    }
}
