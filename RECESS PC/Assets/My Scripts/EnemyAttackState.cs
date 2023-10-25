using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    NavMeshAgent navMeshAgent;

    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().SetBool("Attack", true);
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        Debug.Log("FSM ATTACKING");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.GetComponent<EnemyHealth>().isDead)
        {
            enemy.SwitchState(enemy.deadState);
        }

        else if (enemy.distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            enemy.GetComponent<Animator>().SetBool("Attack", false);
            enemy.SwitchState(enemy.chaseState);
        }

        else if (enemy.distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            enemy.GetComponent<Animator>().SetBool("Attack", true);
        }

        //check distance from player
        //if player is outside attack range, revert to chase state, else remain in attack state
        //if enemy health is <=0, change to dead state
    }
}
