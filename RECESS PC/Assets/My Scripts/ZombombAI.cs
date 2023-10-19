using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ZombombAI : MonoBehaviour
{
    [SerializeField] float chaseRange = 5f;

    [SerializeField] float turnSpeed = 5f;
    [SerializeField] bool TurnOnGizmos = true;
    NavMeshAgent navMeshAgent;
    EnemyHealth health;
    ZombombHealth zombombHealth;
    Transform target;
    float distanceToTarget = Mathf.Infinity;
    public bool isProvoked = false;
    GameObject player;
    ZombieSounds zombieSounds;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        // health = GetComponent<EnemyHealth>();
        zombombHealth = GetComponent<ZombombHealth>();
        zombieSounds = GetComponent<ZombieSounds>();
        StartCoroutine(AllZombieSounds());
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        // if (health.IsDead() == true)
        // {
        //     navMeshAgent.enabled = false;
        //     navMeshAgent.speed = 0;
        //     this.enabled = false;
        //     GetComponent<CapsuleCollider>().enabled = false;
        //     StopAllCoroutines();
        //     // FindObjectOfType<CapsuleCollider>().enabled = false;
        // }
        if (zombombHealth.IsDead() == true)
        {
            navMeshAgent.enabled = false;
            navMeshAgent.speed = 0;
            this.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            zombombHealth.enabled = false;
            StopAllCoroutines();
            // FindObjectOfType<CapsuleCollider>().enabled = false;
        }
        if (target != null)
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);
        }
        // else
        // {
        //     Time.timeScale = 0;
        // }
        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void EngageTarget()
    {
        FaceTarget();
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            GetComponent<Animator>().SetBool("Attack", false);
            GetComponent<Animator>().SetTrigger("Move");
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            GetComponent<Animator>().SetBool("Attack", true);
        }
    }

    private void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        Debug.Log("Chasing");
        if (distanceToTarget < chaseRange)
        {
            isProvoked = false;
            GetComponent<Animator>().SetTrigger("Idle");
            Debug.Log("Back to Patrol");
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        if (TurnOnGizmos == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }

    IEnumerator AllZombieSounds()
    {
        while (true)
        {
            zombieSounds.PlayZombieSounds();
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
            // yield return null;
        }
    }
}
