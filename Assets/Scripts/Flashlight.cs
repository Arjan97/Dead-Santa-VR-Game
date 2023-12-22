using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Flashlight : MonoBehaviour
{
    private bool lightOn = false;
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(ToggleLights);
    }
    private void ToggleLights(ActivateEventArgs args)
    {
        lightOn = !lightOn;

        Transform firstChild = transform.GetChild(0);

        if (firstChild != null)
        {
            firstChild.gameObject.SetActive(lightOn);
        }
    }
}