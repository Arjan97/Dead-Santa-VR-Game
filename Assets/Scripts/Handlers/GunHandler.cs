// Script: GunHandler
// Description: Manages the behavior of a gun in a virtual reality environment, including firing bullets and handling magazines.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class GunHandler : MonoBehaviour
{
    // Prefab for the bullet
    public GameObject bulletPrefab;

    // Spawn point for bullets
    public Transform spawnPoint;

    // Speed of fired bullets
    public float bulletSpeed = 10f;

    // XRBaseInteractor for the gun socket
    public XRBaseInteractor socketInteractor;

    // Magazine attached to the gun
    public Magazine mag;

    // Delay between consecutive shots
    [SerializeField] private float fireDelay = 0.5f; 

    // Flag for rapid fire mode
    [SerializeField] private bool isRapidFireMode = false;
    // Flag to track whether the gun is currently firing
    private bool isFiring = false; 

    // UI element for displaying ammo count
    public TextMeshProUGUI ammoCounterText;

    // UI element for displaying rapid fire mode status
    public TextMeshProUGUI rapidFireText;

    // Start is called before the first frame update
    void Start()
    {
        // Get XRGrabInteractable component
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();

        // Set initial values for UI text elements
        ammoCounterText.text = "24/24";
        rapidFireText.text = "OFF";

        // Add event listeners for grabbable activation and deactivation
        grabbable.activated.AddListener(StartFiring);
        grabbable.deactivated.AddListener(StopFiring);

        // Add event listeners for socket interactor selection
        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);
    }

    void Update()
    {
        // Update the ammo counter text
        UpdateAmmoCounterText();
    }

    // Add a magazine to the gun
    public void AddMagazine(XRBaseInteractable interactor)
    {
        mag = interactor.GetComponent<Magazine>();
        SoundManager.Instance.PlaySoundAtPosition("reload", transform.position);

    }

    // Remove the magazine from the gun
    public void RemoveMagazine(XRBaseInteractable interactor)
    {
        mag = null;
    }

    // Check if the gun can shoot
    public bool CanShoot()
    {
        return mag != null && mag.numberOfBullets > 0;
    }

    // Start firing bullets
    private void StartFiring(ActivateEventArgs args)
    {
        if (mag)
        {
            if (isRapidFireMode && CanShoot())
            {
                FireBullet();
                InvokeRepeating(nameof(FireBullet), 0f, 0.15f);
            }
            else if (CanShoot() && !isFiring)
            {
                StartCoroutine(SingleFire());
            }
        }
    }
    // Coroutine for single fire mode
    private IEnumerator SingleFire()
    {
        FireBullet();
        isFiring = true;
        yield return new WaitForSeconds(fireDelay);
        isFiring = false;
    }

    // Stop firing bullets
    private void StopFiring(DeactivateEventArgs args)
    {
        if (mag != null && mag.numberOfBullets <= 0)
        {
            SoundManager.Instance.PlaySoundAtPosition("magempty", transform.position);
        }

        if (mag == null)
        {
            SoundManager.Instance.PlaySoundAtPosition("nomag", transform.position);
        }

        CancelInvoke(nameof(FireBullet));
    }

    // Fire a bullet
    private void FireBullet()
    {
        if (mag)
        {
            if (CanShoot())
            {
                // Instantiate a bullet
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = spawnPoint.position;

                // Set the velocity of the bullet
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = spawnPoint.forward * bulletSpeed;

                // Destroy the bullet after a certain time
                Destroy(bullet, 5);

                SoundManager.Instance.PlaySoundAtPosition("Gun", transform.position);
                // Decrease the number of bullets in the magazine
                mag.numberOfBullets--;
            }
            else
            {
                StopFiring(null);
            }
        }
    }

    // Set rapid fire mode
    public void SetRapidFireMode(bool enable)
    {
        isRapidFireMode = enable;

        // Update UI text for rapid fire mode status
        if (isRapidFireMode)
        {
            rapidFireText.text = "ON";
        }
        else
        {
            StopFiring(null);
            rapidFireText.text = "OFF";
        }
    }

    // Update the ammo counter text
    private void UpdateAmmoCounterText()
    {
        if (mag)
        {
            if (ammoCounterText != null)
            {
                ammoCounterText.text = mag.numberOfBullets.ToString() + "/24";
            }
            else
            {
                Debug.LogWarning("AmmoCounterText not assigned in the inspector.");
            }
        }
    }
}
