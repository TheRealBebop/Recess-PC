using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] DisplayDamage blood;
    [SerializeField] Weapon gun;
    [SerializeField] Weapon shotgun;
    [SerializeField] GrenadeLauncher grenadeLauncher;
    private void Start()
    {
        blood = GetComponent<DisplayDamage>();
        //Debug.Log("ENABLING DEATH CANVAS");
        gameOverCanvas.enabled = false;
        //Debug.Log("ENABLED DEATH CANVAS SIKKEEEEE");
    }

    public void HandleDeath()
    {
        blood.impactCanvas.enabled = false;
        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        gun.enabled = false;
        shotgun.enabled = false;
        grenadeLauncher.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }
}
