using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon gun;
    public UnityEvent pickupDisplay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // weaponPickedUp = true;
            // Call a function in Weapon.cs to set pickedUp to true
            Debug.Log("SHOTGUUUUUUN");
            pickupDisplay.Invoke();
            gun.EquipWeapon();
            gun.pickedUp = true;
            Destroy(gameObject);
        }
    }
}
