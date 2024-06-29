using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour {

  
    public Collider2D swordCollider;
    Vector2 rightAttackOffset;

    public float damage = 3;
    private void Start() {
        rightAttackOffset = transform.position;
    }
  
    public void AttackRight() {
        print("Attack Right");
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft() {
        print("Attack Left");
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(rightAttackOffset.x*-1, rightAttackOffset.y);
    }

    public void AttackUp(){
        print("Attack Up");
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(0, 0);
    }

    public void AttackDown() {
        print ("Attack Down");
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(0, rightAttackOffset.y*2);
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
                if (enemy != null) {
                    enemy.Health -= damage;
                }
        } 
        else if (other.tag == "Destructable") {
            Debug.Log("Tree hit inside TRIGGER!");
            DestructableScript des = other.GetComponent<DestructableScript>();
            des.TakeDamage(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        Debug.Log("Colliding");
        if (collision.gameObject.tag == "Destructable") {
            Debug.Log("Tree hit!");
            DestructableScript des = collision.gameObject.GetComponent<DestructableScript>();
            des.TakeDamage(damage);
            
        }
    }

    
}


