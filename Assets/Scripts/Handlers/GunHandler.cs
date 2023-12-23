using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GunHandler : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float bulletSpeed = 10f;
    public XRBaseInteractor socketInteractor;
    public Magazine mag;
    [SerializeField] private bool isRapidFireMode = false;
    private bool isFiring = false;

    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(StartFiring);
        grabbable.deactivated.AddListener(StopFiring);

        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);
    }

    public void AddMagazine(XRBaseInteractable interactor)
    {
        mag = interactor.GetComponent<Magazine>();
    }

    public void RemoveMagazine(XRBaseInteractable interactor)
    {
        mag = null;
    }

    public bool CanShoot()
    {
            return mag.numberOfBullets > 0;
        
    }
    private void StartFiring(ActivateEventArgs args)
    {
        if (mag)
        {
            if (isRapidFireMode && CanShoot())
            {
                isFiring = true;
                FireBullet();
                InvokeRepeating(nameof(FireBullet), 0f, 0.15f);
            }
            else if (CanShoot())
            {
                FireBullet();
            }
        }
    }

    private void StopFiring(DeactivateEventArgs args)
    {
        isFiring = false;
        CancelInvoke(nameof(FireBullet));
    }
    private void FireBullet()
    {
        if (mag)
        {
            if (CanShoot())
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = spawnPoint.position;
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = spawnPoint.forward * bulletSpeed;
                Destroy(bullet, 5);

                mag.numberOfBullets--;
            }
            else
            {
                StopFiring(null); // Stop firing if out of ammo
            }
        }
    }

    public void SetRapidFireMode(bool enable)
    {
        isRapidFireMode = enable;

        if (!isRapidFireMode)
        {
            StopFiring(null); 
        }
    }
}
