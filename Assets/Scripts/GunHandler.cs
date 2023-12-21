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

    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);

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

    public void FireBullet(ActivateEventArgs args)
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
                // Out of ammo
                Debug.Log("Out of ammo!");
            }
        }
        
    }
}
