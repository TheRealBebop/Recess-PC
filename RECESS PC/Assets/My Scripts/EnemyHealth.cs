using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 100f;
    NavMeshAgent navMeshAgent;
    ParticleSystem zombieBlood;
    Player gameSession;
    EnemyAI ai;
    ZombieSounds sounds;
    public bool isDead = false;

    private void Start()
    {
        ai = FindObjectOfType<EnemyAI>();
        zombieBlood = GetComponent<ParticleSystem>();
        gameSession = FindObjectOfType<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        sounds = GetComponent<ZombieSounds>();
        // GameObject.DontDestroyOnLoad(this.gameObject);
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
            Die();
            gameSession.AddToScore(1);
            isDead = true;
            // this.enabled = false;
            Destroy(this);
            Destroy(sounds);
            Debug.Log(gameObject.name + " has been killed");
        }
    }
    public void Suicide()
    {
        hitPoints = 0f;
        Die();
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        GetComponent<Animator>().SetTrigger("Die");
        navMeshAgent.enabled = false;
        ai.enabled = false;
    }
}
