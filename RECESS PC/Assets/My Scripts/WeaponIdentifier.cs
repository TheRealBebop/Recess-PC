using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIdentifier : MonoBehaviour
{
    public bool identifiedAsAWeapon = false;
    public bool equippedByDef = false;

    public void Identified()
    {
        identifiedAsAWeapon = true;
    }
}
