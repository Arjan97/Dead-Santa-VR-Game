using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent OnPress;
    public UnityEvent OnRelease;
    GameObject presser;
    public GunHandler gunHandler;
    bool isPressed;
    private bool isRapidFireMode = false;

    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!isPressed)
        {
            button.transform.localPosition = new Vector3(-0.3f, 0, 0);
            presser = other.gameObject;
            OnPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(-1.199f, 0, 0);
            OnRelease.Invoke();
            isPressed = false;
        }
    }

    public void ToggleFireMode()
    {
        isRapidFireMode = !isRapidFireMode;

        if (gunHandler)
        {
            gunHandler.SetRapidFireMode(isRapidFireMode);

            Debug.Log("Switched to " + (isRapidFireMode ? "Rapid-Fire" : "Single-Fire") + " mode.");
        }
    }
}
