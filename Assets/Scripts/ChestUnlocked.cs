using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestUnlocked : MonoBehaviour
{
    public XRBaseInteractor socketInteractor;
    private Rigidbody parentRB;
    void Start()
    {
        socketInteractor.onSelectEntered.AddListener(OpenChest);
        parentRB = transform.parent.GetComponent<Rigidbody>();
        parentRB.freezeRotation = true;

    }

    private void OpenChest(XRBaseInteractable interactor)
    {
        if (parentRB != null)
        {
            parentRB.freezeRotation = false;

        }
    }
}
