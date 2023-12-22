using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flashlight : MonoBehaviour
{
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(ToggleLights);
    }

    private void ToggleLights(ActivateEventArgs args)
    {
       GameObject.GetChildren().ForEach(child => child.SetActive(!child.activeSelf));
    }
}
