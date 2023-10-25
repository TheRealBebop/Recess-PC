using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState currentState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyDeadState deadState = new EnemyDeadState();

    public Transform[] waypoints;
    public GameObject player;
    public bool isProvoked = false;

    EnemyPatrol patrol;
    ZombieSounds zombieSounds;

    [SerializeField] public float chaseRange = 5f;
    [SerializeField] public float turnSpeed = 5f;
    [SerializeField] public bool TurnOnGizmos = true;
    public float distanceToTarget;

    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);

        zombieSounds = GetComponent<ZombieSounds>();
        StartCoroutine(AllZombieSounds());

        patrol = GetComponent<EnemyPatrol>();
        waypoints = patrol.waypoints;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform != null)
        {
            distanceToTarget = Vector3.Distance(player.transform.position, transform.position);
        }
    }

    void Update()
    {
        currentState.UpdateState(this);
        distanceToTarget = Vector3.Distance(player.transform.position, transform.position);
        Provoked();
    }

    public void Provoked()
    {
        if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
        else if (distanceToTarget > chaseRange)
        {
            isProvoked = false;
        }
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void OnDrawGizmosSelected()
    {
        if (TurnOnGizmos == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }

    public void KillAll()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        //Destroy(this);
    }

    IEnumerator AllZombieSounds()
    {
        while (true)
        {
            zombieSounds.PlayZombieSounds();
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
        }
    }
}
