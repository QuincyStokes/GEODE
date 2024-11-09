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
    [SerializeField] public float damage;

    public float Damage 
    {
        get {return damage;}
        set {damage = value;}
    }

    public bool IsAttacking
    {
        get {return isAttacking;}
        set {isAttacking = value;}
    }

    private Vector3 lungeDirection;
    private float lungeTime = 0;
    private float recoilTime = 0;
    public bool isAttacking = false;

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
        

        isAttacking = true;

        //rb.AddForce(lungeDirection * lungeSpeed, ForceMode2D.Impulse);
        rb.velocity = lungeDirection * lungeSpeed;
        // while (lungeTime < lungeDuration) {
        //     transform.position += lungeDirection * lungeSpeed * Time.deltaTime;
        //     lungeTime += Time.deltaTime;

        yield return new WaitForSeconds(lungeDuration);
        // }
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        isAttacking = false;
        navMeshAgent.enabled = true;
        stateManager.SetState(EnemyStateManager.EnemyState.Chasing);
    }

    
    
}
