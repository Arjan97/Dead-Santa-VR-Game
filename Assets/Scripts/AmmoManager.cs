using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] public int maxAmmo = 24; // Total ammo count
    [SerializeField] public int maxMagazine = 6; // Maximum magazine size
    [SerializeField] private int currentAmmo; // Current total ammo count
    private int currentMagazine; // Current magazine size
    private XRBaseInteractor socketInteractor;

    void Start()
    {
        currentAmmo = maxAmmo;
        currentMagazine = maxMagazine;
        socketInteractor.OnSelectEnter.AddListener(AddMagazine);
        socketInteractor.OnSelectEnter.AddListener(RemoveMagazine);
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetCurrentMagazine()
    {
        return currentMagazine;
    }

    public void AddMagazine(XRBaseInteractable interactor)
    {
        currentAmmo = maxMagazine;
    }
    
    public void RemoveMagazine(XRBaseInteractable interactor) 
    {
        currentAmmo = 0;
    }

    public void Slide(XRBaseInteractable interactor)
    {

    }

    public bool CanShoot()
    {
        return currentMagazine > 0;
    }

    public void Shoot()
    {
        if (CanShoot() && mag != null)
        {
            currentMagazine--;
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }
}
