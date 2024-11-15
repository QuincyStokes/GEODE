using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   public float speed = 10f;
   public float lifeTime = 2f;
   public int damage = 5;

   void Start() {
        Destroy(gameObject, lifeTime);
   }

   void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.CompareTag("Enemy")) {
        EnemyCollision es = collision.gameObject.GetComponent<EnemyCollision>();
        EnemyHealthController ehc = collision.gameObject.GetComponent<EnemyHealthController>();
        ehc.TakeDamage(damage);
        es.StartKnockback(transform.position);
        es.StartDamageColorChange();
        Destroy(gameObject);
    }
   }
}
