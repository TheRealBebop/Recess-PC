using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrenadeLauncherPickup : MonoBehaviour
{
    public bool weaponPickedUp = false;
    [SerializeField] GrenadeLauncher GrenadeLauncher;
    [SerializeField] WeaponSwitcher weaponSwitcher;
    [SerializeField] WeaponIdentifier weaponIdentifier;
    GrenadeLauncher launcher;
    public UnityEvent pickupDisplay;

    private void Start()
    {
        launcher = FindObjectOfType<GrenadeLauncher>();
        //weaponIdentifier = FindObjectOfType<WeaponIdentifier>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // weaponPickedUp = true;
            // Call a function in Weapon.cs to set pickedUp to true
            pickupDisplay.Invoke();
            GrenadeLauncher.EquipWeapon();
            GrenadeLauncher.pickedUp = true;
            weaponIdentifier.Identified();
            weaponSwitcher.numberOfWeapons++;
            Destroy(gameObject);
        }
    }
}