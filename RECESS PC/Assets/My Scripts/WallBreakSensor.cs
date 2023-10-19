using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreakSensor : MonoBehaviour
{

    Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = FindObjectOfType<Player>();
            player.BreakWall();
        }
    }
}
