using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class EnemyCollision : MonoBehaviour
{

    private IAttack Attack;
    private NavMeshAgent navMeshAgent;
    private EnemyStateManager stateManager;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private float knockbackPower;


    public void Start() {
        Attack = GetComponent<IAttack>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateManager = GetComponent<EnemyStateManager>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

    }
    void OnCollisionEnter2D(Collision2D other)
    {  
        print("HIT");
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Obstacle"))
        {
            print("PLAYER OR OBSTACLE");
            StopCoroutine(Attack.Attack(Vector3.zero));
            Attack.IsAttacking = false;
            StartCoroutine(Knockback(other.gameObject.transform.position));
            other.gameObject.GetComponent<PlayerHealthManager>().TakeDamage(Attack.Damage); 
            // ^ should make a playerHealth Interface to implement these 
            // so we dont have to do seprate check for what we hit
            if(other.gameObject.CompareTag("Player"))
            {
                //damage the player
            }
        }
        else if (other.gameObject.CompareTag("Swingable"))
        {
            print("SWINGALBEEE");
            //StartCoroutine(Knockback(other.gameObject.transform.position));
        }
    }

    public IEnumerator Knockback(Vector3 position) {
        print("Knockback Started");

        stateManager.currentState = EnemyStateManager.EnemyState.Knockback;
        navMeshAgent.enabled = false;
        
        Vector3 direction = position - transform.position;
        direction.z = 0f;
        direction *= -1;

        Vector3 recoilDirection = direction.normalized;
        rb.isKinematic = false;
        rb.velocity = recoilDirection * knockbackPower;
        print("SETTING VELOCITY");
        
                                        //no idea what to multiply this by
        yield return new WaitForSeconds(1);
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        print("VELOCITY RESET TO 0");
        

        navMeshAgent.enabled = true;
        stateManager.currentState = EnemyStateManager.EnemyState.Idle;
        print("Knockback Ended");
    }

    public IEnumerator DamageColorChange() {
        Color originalColor = sr.color;
        sr.color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(.2f);
        sr.color = originalColor;
    }

    

}
