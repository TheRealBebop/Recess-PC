using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombPickup : MonoBehaviour
{
    [SerializeField] public int ammo;
    [SerializeField] GrenadeLauncher Launcher;
    public UnityEvent pickupDisplay;

    void Start()
    {
        // Launcher = FindObjectOfType<GrenadeLauncher>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickupDisplay.Invoke();
            Launcher.IncreaseAmmo(ammo);
            Destroy(gameObject);
            // StartCoroutine(DisplayPickupCanvas());
        }
    }
}
