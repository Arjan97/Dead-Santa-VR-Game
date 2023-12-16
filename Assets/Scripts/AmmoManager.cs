using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] public int maxAmmo = 24; // Total ammo count
    [SerializeField] public int maxMagazine = 6; // Maximum magazine size
    [SerializeField] private int currentAmmo; // Current total ammo count
    private int currentMagazine; // Current magazine size

    void Start()
    {
        currentAmmo = maxAmmo;
        currentMagazine = maxMagazine;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetCurrentMagazine()
    {
        return currentMagazine;
    }

    public void Reload()
    {
        int bulletsToReload = Mathf.Min(maxMagazine - currentMagazine, currentAmmo);
        currentMagazine += bulletsToReload;
        currentAmmo -= bulletsToReload;
    }

    public bool CanShoot()
    {
        return currentMagazine > 0;
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            currentMagazine--;
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }
}
