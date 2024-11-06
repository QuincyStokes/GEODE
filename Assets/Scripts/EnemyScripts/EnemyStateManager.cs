using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyState currentState = EnemyState.Idle;
    public enum EnemyState
    {
        Idle = 0,
        Chasing = 1,
        Attacking = 2,
        Lunging = 3, 
        Shooting = 4,
        Recoiling = 5,
        Dead = 6
    }

    public void SetState(EnemyState state) {
        currentState = state;
    }

    public EnemyState GetState(EnemyState state) {
        return currentState;
    }

    
}
