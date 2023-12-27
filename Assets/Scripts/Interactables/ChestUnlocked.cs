// Script: ChestUnlocked
// Description: Handles the unlocking of a chest using XR interactions.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestUnlocked : MonoBehaviour
{
    // Reference to the XRBaseInteractor for interactions
    public XRBaseInteractor socketInteractor;

    // Reference to the parent Rigidbody of the chest
    private Rigidbody parentRB;

    // Reference to the RewardManager
    public RewardManager rewardManager; 

    void Start()
    {
        // Subscribe to the onSelectEntered event of the XRBaseInteractor
        socketInteractor.onSelectEntered.AddListener(OpenChest);

        rewardManager = FindObjectOfType<RewardManager>();
        // Get the parent Rigidbody component
        parentRB = transform.parent.GetComponent<Rigidbody>();

        // Freeze rotation initially
        parentRB.freezeRotation = true;
    }

    // Called when the chest is selected by an XR interactor
    private void OpenChest(XRBaseInteractable interactor)
    {
        // Check if the parent Rigidbody is not null
        if (parentRB != null)
        {
            // Allow rotation after the chest is unlocked
            parentRB.freezeRotation = false;
            // Call the ShowReward method in the RewardManager
            rewardManager.ShowReward();
        }
    }
}
