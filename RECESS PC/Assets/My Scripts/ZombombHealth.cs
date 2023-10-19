using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombombHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 100f;
    [SerializeField] ParticleSystem zombieBlood;
    Player gameSession;
    [SerializeField] ParticleSystem explosion;
    ZombombExplosion explode;
    ZombombAI ai;
    ZombombAttack attack;
    NavMeshAgent nav;
    ZombieSounds sounds;
    // EnemyAI ai;
    public bool isDead = false;
    ParticleSystem tempexplosion;

    private void Start()
    {
        gameSession = FindObjectOfType<Player>();
        explode = GetComponent<ZombombExplosion>();
        ai = GetComponent<ZombombAI>();
        attack = GetComponent<ZombombAttack>();
        nav = GetComponent<NavMeshAgent>();
        sounds = GetComponent<ZombieSounds>();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        BroadcastMessage("OnDamageTaken");
        zombieBlood.Play(true);
        if (hitPoints <= 0)
        {
            explosion.Play(true);
            Die();
            if (GetComponent<ZombombAttack>().suicide != true)
            {
                gameSession.AddToScore(1);
            }
            isDead = true;
            Destroy(this);
            Destroy(ai);
            Destroy(explode);
            Destroy(attack);
            Destroy(sounds);
            nav.enabled = false;
            Debug.Log(gameObject.name + " has been killed");
        }
    }

    public void Suicide()
    {
        isDead = true;
        hitPoints = 0f;
        // if (isDead)
        // {
        //     return;
        // }
        GetComponent<Animator>().SetTrigger("Die");
        // this.enabled = false;
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }
        Debug.Log(gameObject.name + " In die rn");
        isDead = true;
        Debug.Log(gameObject.name + " Isdead set to true");
        GetComponent<Animator>().SetTrigger("Die");
        Debug.Log(gameObject.name + " Die trigger set to true");
        explode.Explode();
        // tempexplosion.Play();
        // navMeshAgent.enabled = false;
        // ai.enabled = false;
    }
}
