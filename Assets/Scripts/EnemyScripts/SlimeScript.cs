using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    Animator animationManager;
    public AudioSource slimeJump; 
    public GameObject player;
    public float moveSpeed;
    private Rigidbody2D rb;
    private ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffset = 0.05f;

    public void Start() {
        animationManager = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    public void Update() {
        // Calculate the direction from the enemy to the player
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Move the enemy towards the player
        bool success = TryMove(direction);
                if(!success) {
                    success = TryMove(new Vector2(direction.x, 0));

                    if(!success) {
                        success = TryMove(new Vector2(0, direction.y));
                    }
                }
       
    }
    public float Health{
        set {
            health = value;
            if(health <= 0) {

                Defeated();
            }
        } get {
            return health;
        }
    }
    public float health = 5;
   
    public void Defeated() {
        animationManager.SetTrigger("Defeated");
        slimeJump.Play();
    }    

    public void RemoveEnemy() {
        Destroy(gameObject);
    }

    private bool TryMove(Vector2 direction) {
        int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
        if(count == 0) {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        } 
        //Debug.Log("NOPE");
        return false;

    }
}
