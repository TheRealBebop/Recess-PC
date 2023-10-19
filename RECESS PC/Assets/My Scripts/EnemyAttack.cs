using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Player target;
    DisplayDamage player;
    EnemyHealth health;

    [SerializeField] float damage = 40f;

    void Start()
    {
        target = FindObjectOfType<Player>();
        player = FindObjectOfType<DisplayDamage>();
        health = FindObjectOfType<EnemyHealth>();
    }

    public void AttackHitEvent()
    {
        if (target == null)
        {
            return;
        }
        target.TakeDamage(damage);
        player.GetComponent<DisplayDamage>().ShowDamageImpact();
    }

}
