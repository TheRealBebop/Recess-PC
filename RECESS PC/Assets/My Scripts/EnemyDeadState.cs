using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public bool enemyIsDead = false;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("FSM DEATH");
        enemy.GetComponent<Animator>().SetTrigger("Die");
        enemyIsDead = true;
        enemy.KillAll();
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        
    }
}
