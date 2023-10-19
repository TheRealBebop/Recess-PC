using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class AmmoPickup : MonoBehaviour
{

    [SerializeField] int ammoAmount = 5;
    [SerializeField] AmmoType ammoType;

    public UnityEvent pickupDisplay;
    // public GameObject pickupCanvas;
    Ammo GunAmmo;

    void Start()
    {
        // pickupCanvas.SetActive(true);
        // finalText.text = "+" + ammoAmount.ToString() + pickupText.text;
        GunAmmo = FindObjectOfType<Ammo>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickupDisplay.Invoke();
            GunAmmo.IncreaseCurrentAmmo(ammoType, ammoAmount);
            Destroy(gameObject);
            // StartCoroutine(DisplayPickupCanvas());
        }
    }
}