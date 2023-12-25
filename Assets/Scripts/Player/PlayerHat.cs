// Script: PlayerHat
// Description: Manages the behavior of a player's hat using XR interactions.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHat : MonoBehaviour
{
    // The tag associated with the hat (e.g., "Santa")
    public string hatTag = "Santa";

    // Indicates whether the hat is currently equipped
    public bool isEquipped = false;

    // Reference to the XR socket interactor for the hat
    private XRSocketInteractor hatSocketInteractor;

    // Reference to the player GameObject
    private GameObject player;

    // The original parent of the hat before being equipped
    private Transform originalParent;

    void Start()
    {
        // Find the player GameObject with the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player");
        hatSocketInteractor = GameObject.FindGameObjectWithTag("PlayerHeadSocket").GetComponent<XRSocketInteractor>();

        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }

        // Set the hat's tag to the specified hatTag
        gameObject.tag = hatTag;

        // Check if the hatSocketInteractor is assigned
        if (hatSocketInteractor == null)
        {
            Debug.LogWarning("Hat Socket Interactor not assigned to PlayerHat script.");
        }
        else
        {
            // Subscribe to events when the hat is equipped or removed
            hatSocketInteractor.onSelectEntered.AddListener(OnHatEquipped);
            hatSocketInteractor.onSelectExited.AddListener(OnHatRemoved);
        }
    }

    // Called when the hat is equipped
    private void OnHatEquipped(XRBaseInteractable interactable)
    {
        // Store the original parent before changing it to the player's transform
        originalParent = transform.parent;

        // Set the hat's parent to the player's transform
        transform.parent = player.transform;

        // Update the equipped status
        isEquipped = true;

        Debug.Log("Hat Equipped!");
    }

    // Called when the hat is removed
    private void OnHatRemoved(XRBaseInteractable interactable)
    {
        // Restore the hat's original parent
        transform.parent = originalParent;

        // Update the equipped status
        isEquipped = false;

        Debug.Log("Hat Removed!");
    }
}
