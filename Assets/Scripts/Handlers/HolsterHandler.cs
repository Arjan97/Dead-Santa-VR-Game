using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HolsterHandler : MonoBehaviour
{
    // Reference to the XRBaseInteractor for the holster
    public XRBaseInteractor holsterInteractor;

    private void Start()
    {
        // Add event listener for holster selection
        holsterInteractor.onSelectEntered.AddListener(HolsterObject);
    }

    private void HolsterObject(XRBaseInteractable interactor)
    {
        // Play the holster sound when an object is holstered
        SoundManager.Instance.PlaySoundAtPosition("holster", transform.position);
    }
}
