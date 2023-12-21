using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHat : MonoBehaviour
{
    public string hatTag = "Santa";
    public bool isEquipped = false; 
    public XRSocketInteractor hatSocketInteractor;
    private GameObject player;
    private Transform originalParent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }

        gameObject.tag = hatTag;
        if (hatSocketInteractor == null)
        {
            Debug.LogWarning("Hat Socket Interactor not assigned to PlayerHat script.");
        }
        else
        {
            hatSocketInteractor.onSelectEntered.AddListener(OnHatEquipped);
            hatSocketInteractor.onSelectExited.AddListener(OnHatRemoved);
        }
    }

    private void OnHatEquipped(XRBaseInteractable interactable)
    {
        originalParent = transform.parent; 
        transform.parent = player.transform; 
        isEquipped = true;
        Debug.Log("Hat Equipped!");
    }

    private void OnHatRemoved(XRBaseInteractable interactable)
    {
        transform.parent = originalParent;
        isEquipped = false;
        Debug.Log("Hat Removed!");
    }
}
