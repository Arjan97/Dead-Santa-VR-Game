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
    private GameObject playerHead;

    // The original parent of the hat before being equipped
    private Transform originalParent;

    void Start()
    {
        // Find the player GameObject with the "PlayerHeadSocket" tag
        playerHead = GameObject.FindGameObjectWithTag("PlayerHeadSocket");
        hatSocketInteractor = GameObject.FindGameObjectWithTag("PlayerHeadSocket").GetComponent<XRSocketInteractor>();

        if (playerHead == null)
        {
            Debug.LogError("Playerhead not found. Make sure the player has the 'PlayerHeadSocket' tag.");
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
        // Check if the player already has a child (hat) to prevent multiple equips
        if (playerHead.transform.childCount == 1)
        {
            // Store the original parent before changing it to the player's transform
            originalParent = transform.parent;

            // Set the hat's parent to the player's transform
            transform.parent = playerHead.transform;
            // Update the equipped status
            isEquipped = true;
            Debug.Log("Hat Equipped!");
        }
        else
        {
            Debug.Log("Already have a hat equipped dummy.");
        }
    }

    // Called when the hat is removed
    private void OnHatRemoved(XRBaseInteractable interactable)
    {
        // Check if the originalParent is not null before setting it as the parent
        if (originalParent != null)
        {
            // Restore the hat's original parent
            transform.parent = originalParent;
        }

        // Update the equipped status
        isEquipped = false;

        Debug.Log("Hat Removed!");
    }

}
