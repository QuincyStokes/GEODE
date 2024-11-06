using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateManager))]
public class EnemyHealthController : MonoBehaviour
{

    private EnemyStateManager stateManager;

    //Lets do getter and setter for health
    private float Health {
        set {
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        } get {
            return health;
        }
    }
    [SerializeField]private float health;

    
    
    private void Defeated() 
    {
        stateManager.currentState = EnemyStateManager.EnemyState.Dead;
        RemoveEnemy();
    }

    private void RemoveEnemy() 
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage) {
        Health -= damage;
    }

}
