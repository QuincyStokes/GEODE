using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR.Haptics;
 
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStateManager))]
public class LungeAttack : MonoBehaviour, IAttack
{
    Rigidbody2D rb;
    EnemyStateManager stateManager;
    NavMeshAgent navMeshAgent;
    [SerializeField] private float lungeSpeed;
    [SerializeField] private float lungeDuration;
    [SerializeField] private float recoilSpeed;
    [SerializeField] private float recoilDuration;
    

    private Vector3 lungeDirection;
    private float lungeTime = 0;
    private float recoilTime = 0;
    private bool isLunging = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        stateManager = GetComponent<EnemyStateManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    //Lunge attack
    public IEnumerator Attack(Vector3 position) {
        stateManager.currentState = EnemyStateManager.EnemyState.Lunging;
        navMeshAgent.enabled = false;

        rb.isKinematic = false;

        Vector3 direction = position - transform.position;
        direction.z = 0f;
        lungeDirection = direction.normalized;
        

        isLunging = true;

        //rb.AddForce(lungeDirection * lungeSpeed, ForceMode2D.Impulse);
        rb.velocity = lungeDirection * lungeSpeed;
        // while (lungeTime < lungeDuration) {
        //     transform.position += lungeDirection * lungeSpeed * Time.deltaTime;
        //     lungeTime += Time.deltaTime;

        yield return new WaitForSeconds(lungeDuration);
        // }
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        isLunging = false;
        navMeshAgent.enabled = true;
        stateManager.SetState(EnemyStateManager.EnemyState.Chasing);
    }

    private IEnumerator Recoil(Vector3 position) {
        print("RECOILING");
        navMeshAgent.enabled = false;
        stateManager.currentState = EnemyStateManager.EnemyState.Recoiling;

        Vector3 direction = position - transform.position;
        direction.z = 0f;
        direction *= -1;

        Vector3 recoilDirection = direction.normalized;
        rb.isKinematic = false;
        rb.velocity = recoilDirection * recoilSpeed;
        rb.isKinematic = true;
        yield return new WaitForSeconds(recoilDuration);
        rb.velocity = Vector3.zero;

        navMeshAgent.enabled = true;
        stateManager.currentState = EnemyStateManager.EnemyState.Chasing;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        print("COLLIDED");
        if(isLunging)
        {   
            print("LUNGING");
            if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Obstacle"))
            {
                StopCoroutine(Attack(Vector3.zero));
                isLunging = false;
                StartCoroutine(Recoil(other.gameObject.transform.position));

                if(other.gameObject.CompareTag("Player"))
                {
                    //damage the player
                }
            }
           

        }
     }
}
