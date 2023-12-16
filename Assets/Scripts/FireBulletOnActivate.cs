using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class FireBulletOnActivate : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float bulletSpeed = 10f;
    public XRBaseInteractor socketInteractor;
    public Magazine mag;
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);

        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExit.AddListener(RemoveMagazine);
    }

    public void AddMagazine(XRBaseInteractable interactor)
    {
        mag = interactor.GetComponent<Magazine>();
        //mag.numberOfBullets = maxMagazine;
    }

    public void RemoveMagazine(XRBaseInteractable interactor)
    {
        mag = null;
    }

    public void Slide(XRBaseInteractable interactor)
    {

    }

    public bool CanShoot()
    {
        if (mag != null)
        {
            return mag.numberOfBullets > 0;
        }
        else { return false; }
    }

    public void FireBullet(ActivateEventArgs args)
    {
        if (CanShoot() && mag != null)
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
