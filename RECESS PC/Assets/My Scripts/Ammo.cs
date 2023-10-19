using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] AmmoSlot[] ammoSlots;

    [System.Serializable]
    public class AmmoSlot
    {
        public AmmoType ammoType;
        public int ammoAmount;
    }

    private void Start()
    {
        // GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public int GetAmmoAmount(AmmoType ammoType)
    {
        return GetAmmoSlot(ammoType).ammoAmount;
    }

    public void ReduceCurrentAmmo(AmmoType ammoType)
    {
        GetAmmoSlot(ammoType).ammoAmount--;
    }
    public void IncreaseCurrentAmmo(AmmoType ammoType, int newAmmoAmount)
    {
        GetAmmoSlot(ammoType).ammoAmount += newAmmoAmount;
    }

    public AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (AmmoSlot slot in ammoSlots)
        {
            if (slot.ammoType == ammoType)
            {
                return slot;
            }
        }
        return null;
    }

}
