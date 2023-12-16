using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRSocketInteractorTag : XRSocketInteractor
{
    public string targetTag;

    //Make sure the interactable has the tag we want so we can't do weird things like attach a rock to the gun.
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag(targetTag);
    }
}
