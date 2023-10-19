using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPickup : MonoBehaviour
{
    public UnityEvent pickupDisplay;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player.playerHealth < 100f)
            {
                pickupDisplay.Invoke();
                player.Heal();
                Destroy(gameObject);
            }
        }
    }
}
