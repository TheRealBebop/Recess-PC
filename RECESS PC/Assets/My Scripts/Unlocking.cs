using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Unlocking : MonoBehaviour
{
    [SerializeField] SceneSwitcher door;
    public UnityEvent pickupDisplay;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickupDisplay.Invoke();
            door.locked = false;
            Destroy(gameObject);
        }
    }
}
