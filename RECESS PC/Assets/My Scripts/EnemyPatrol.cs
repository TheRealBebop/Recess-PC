using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;
    public int waypointIndex;
    Vector3 target;

    bool isAttacking;
    void Start()
    {
        isAttacking = GetComponent<EnemyAI>().isProvoked;
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        FaceTarget();
    }

    void Update()
    {
        if (isAttacking != true)
        {
            if (Vector3.Distance(transform.position, target) < 2)
            {
                IterateWaypointIndex();
                UpdateDestination();
            }
        }
    }

    private void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;
        Debug.Log("Waypoint number " + waypointIndex);
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
        FaceTarget();
    }
    private void FaceTarget()
    {
        Vector3 direction = (waypoints[waypointIndex].position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, waypoints[waypointIndex].rotation, Time.deltaTime * 100f);
    }
}
