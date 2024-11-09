using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.MLAgents.SideChannels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;



/// <summary>
/// Move towards player, initiate attack if has one
/// Reliant on NavMesh agent
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStateManager))]
[RequireComponent(typeof(IAttack))]
public class MoveTowardsPlayer : MonoBehaviour
{   
    //Pretty much everything is controlled through the agent component
    private NavMeshAgent agent;
    private GameObject player; 
    private IAttack attackManager;
    private EnemyStateManager stateManager;

    [SerializeField] private float attackCooldown;
    private float attackTimer;


    public void Start() 
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        attackManager = GetComponent<IAttack>();
        stateManager = GetComponent<EnemyStateManager>();
        //Not actually sure what these do
        if(agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;     
        }
        
    }

    public void Update()
    {
        //for now calling this every frame, may change it to update
        //every x amount of time in the future
        if(player != null) 
        {
            if(attackTimer > 0) {
                attackTimer -= Time.deltaTime;
            }

            float distanceBetween = Vector3.Distance(
                new Vector3(player.transform.position.x, player.transform.position.y, 0), new Vector3(transform.position.x, transform.position.y, 0));
           

            ///STATE MACHINE FOR ENEMY DECISION
            switch (stateManager.currentState)
            {
                case EnemyStateManager.EnemyState.Idle:
                    if(agent.enabled == true) {
                        agent.ResetPath();
                    }
                   
                    break;
                
                case EnemyStateManager.EnemyState.Chasing:
                    agent.SetDestination(player.transform.position);
                    break;
                
                case EnemyStateManager.EnemyState.Attacking:
                    
                    StartCoroutine(attackManager.Attack(player.transform.position));
                    attackTimer = attackCooldown;
                    break;

                case EnemyStateManager.EnemyState.Lunging:
                    break; //havent implemented lunging

                case EnemyStateManager.EnemyState.Shooting:
                    break; //havent implemented shooting at all

                case EnemyStateManager.EnemyState.Knockback:
                    break; //havent implemented recoiling

                case EnemyStateManager.EnemyState.Dead:
                    break;

            }


            //************STATE UPDATING LOGIC

            //Can only attack from either idle or chasing
            if(distanceBetween < .5 
            && (stateManager.currentState == EnemyStateManager.EnemyState.Chasing
            || stateManager.currentState == EnemyStateManager.EnemyState.Idle)
            && attackTimer <= 0) 
            { 
                stateManager.SetState(EnemyStateManager.EnemyState.Attacking);
            } 
            //can only chase from idle 
            //THIS NUMBER REPRESENTS THEIR "AGGRO" RANGE
            else if (distanceBetween >= .5 && distanceBetween <= 2 && stateManager.currentState == EnemyStateManager.EnemyState.Idle )
            
            {
                stateManager.SetState(EnemyStateManager.EnemyState.Chasing);
            }
            //Can only idle from chasing
            else if (distanceBetween > 2)
            {
                stateManager.SetState(EnemyStateManager.EnemyState.Idle);
            }
        }
    }
}
