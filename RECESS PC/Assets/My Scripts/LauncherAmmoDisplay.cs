using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LauncherAmmoDisplay : MonoBehaviour
{
    [SerializeField] GrenadeLauncher launcher;
    [SerializeField] TextMeshProUGUI ammoText;

    // Update is called once per frame
    void Update()
    {
        DisplayAmmo();
    }

    private void DisplayAmmo()
    {
        if (launcher.pickedUp == true)
            ammoText.text = launcher.ammo.ToString();
    }
}
