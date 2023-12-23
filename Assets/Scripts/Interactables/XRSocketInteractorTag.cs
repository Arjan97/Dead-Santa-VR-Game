// Script: XRSocketInteractorTag
// Description: Extends XRSocketInteractor to filter interactables based on their tags.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorTag : XRSocketInteractor
{
    // The target tag that interactables must have to be selected
    public string targetTag;

    // Overrides the CanSelect method to include tag filtering
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // Calls the base CanSelect method and checks the tag of the interactable
        return base.CanSelect(interactable) && interactable.CompareTag(targetTag);
    }
}
