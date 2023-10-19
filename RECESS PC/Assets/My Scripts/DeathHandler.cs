using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;
    DisplayDamage blood;
    private void Start()
    {
        blood = GetComponent<DisplayDamage>();
        Debug.Log("ENABLING DEATH CANVAS");
        gameOverCanvas.enabled = false;
        Debug.Log("ENABLED DEATH CANVAS SIKKEEEEE");
    }

    public void HandleDeath()
    {
        gameOverCanvas.enabled = true;
        blood.impactCanvas.enabled = false;
        Time.timeScale = 0;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }
}
