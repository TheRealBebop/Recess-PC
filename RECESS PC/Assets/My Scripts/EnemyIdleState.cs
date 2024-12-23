using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    NavMeshAgent navMeshAgent;
    public int waypointIndex;
    Vector3 target;

    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().SetTrigger("Idle");
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        target = enemy.waypoints[waypointIndex].position;
        UpdateDestination(enemy);
        Debug.Log("FSM PATROL");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (!enemy.isProvoked)
        {
            enemy.GetComponent<Animator>().SetTrigger("Idle");
            if (Vector3.Distance(enemy.transform.position, target) < 2)
            {
                IterateWaypointIndex(enemy);
                UpdateDestination(enemy);
                Debug.Log("THIS IS IDLE STATE");
            }
        }


        else if (enemy.isProvoked)
        {
            {
                enemy.SwitchState(enemy.chaseState);
            }
        }

        if (enemy.GetComponent<EnemyHealth>().isDead)
        {
            enemy.SwitchState(enemy.deadState);
            //enemy.KillAll();
        }
        //check distance from player
        //if player is outside chase range, stay in idle state
        //if player is within chase state change to chase state
        //OPTIONAL: if enemy health is decreased (if enemy is hit) in idle state, change to chase state
    }


    public void IterateWaypointIndex(EnemyStateManager enemy)
    {
        waypointIndex++;
        Debug.Log("Waypoint number " + waypointIndex);
        if (waypointIndex == enemy.waypoints.Length)
        {
            waypointIndex = 0;
        }
        FaceWaypoint(enemy);
    }

    private void FaceWaypoint(EnemyStateManager enemy)
    {
        Vector3 direction = (enemy.waypoints[waypointIndex].position - enemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        enemy.transform.rotation = lookRotation;
    }

    private void UpdateDestination(EnemyStateManager enemy)
    {
        target = enemy.waypoints[waypointIndex].position;
        navMeshAgent.SetDestination(target);
    }
}
