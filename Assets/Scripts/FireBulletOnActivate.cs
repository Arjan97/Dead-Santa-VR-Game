using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class FireBulletOnActivate : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float bulletSpeed = 10f;
    public AmmoManager ammoManager;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    void Update()
    {

    }

    public void FireBullet(ActivateEventArgs args)
    {
        if (ammoManager.CanShoot())
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = spawnPoint.position;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = spawnPoint.forward * bulletSpeed;
            Destroy(bullet, 5);

            ammoManager.Shoot();
        }
        else
        {
            // Out of ammo
        }
    }
}
