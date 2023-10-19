using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombombAttack : MonoBehaviour
{
    Player target;
    DisplayDamage player;
    ZombombHealth health;
    // ZombombExplosion explode;
    // GameObject explosion;
    public ParticleSystem explosion;
    public bool suicide = false;
    [SerializeField] float damage = 70f;

    void Start()
    {
        target = FindObjectOfType<Player>();
        player = FindObjectOfType<DisplayDamage>();
        health = GetComponent<ZombombHealth>();
        // explode = FindObjectOfType<ZombombExplosion>();
        // explosion = GameObject.Find("Zombomb Explosion Effect");
    }

    public void ZombombAttackHitEvent()
    {
        if (target == null)
        {
            return;
        }
        // health.Suicide();
        // health.isDead = true;
        suicide = true;
        health.TakeDamage(100f);
        player.GetComponent<DisplayDamage>().ShowDamageImpact();
        target.TakeDamage(damage);
    }

    public void ZombombExplosionEvent()
    {
        explosion.Play();
        // explode.Explode();
    }

    public void ZombombExplosionStopEvent()
    {
        explosion.Stop();
    }
}
