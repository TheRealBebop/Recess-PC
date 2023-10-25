using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyChaseState : EnemyBaseState
{
    //public bool isProvoked = false;
    NavMeshAgent navMeshAgent;
    Transform target;

    public override void EnterState(EnemyStateManager enemy)
    {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        target = enemy.player.transform;
        enemy.GetComponent<Animator>().SetTrigger("Move");
        Debug.Log("FSM CHASE INITIATED");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            enemy.SwitchState(enemy.attackState);
        }

        else if(enemy.distanceToTarget >= navMeshAgent.stoppingDistance && enemy.distanceToTarget < enemy.chaseRange) 
        {
            enemy.GetComponent<Animator>().SetTrigger("Move");
            FaceTarget(enemy);
            navMeshAgent.SetDestination(target.position);
        }

        else if(enemy.distanceToTarget > enemy.chaseRange)
        {
            enemy.SwitchState(enemy.idleState);
            enemy.GetComponent<Animator>().SetTrigger("Idle");
            enemy.isProvoked = false;
        }

        if (enemy.GetComponent<EnemyHealth>().isDead)
        {
            enemy.SwitchState(enemy.deadState);
            //enemy.KillAll();
        }

        //check distance from player
        //if player is outside chase range, revert to idle state 
        //if player is inside attack range, change to attack state 
    }

    public void FaceTarget(EnemyStateManager enemy)
    {
        Vector3 direction = (target.position - enemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * enemy.turnSpeed);
    }
}
