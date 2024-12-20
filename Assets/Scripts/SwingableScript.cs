using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingableScript : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other) {
        print("Hit something!");
        if(other.tag == "Enemy") {
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null) {
                enemy.TakeDamage(ItemManager.instance.currentItem.damage);
            }
            //  YUCKY WAY TO DO THIS..
            EnemyCollision enemyCol = other.GetComponent<EnemyCollision>();
            if(enemyCol)
            {   
                enemyCol.StartKnockback(transform.position);
                enemyCol.StartDamageColorChange();
                
            }
        } 
        else if (other.tag == "Destructable") {
            //"other" is probably the whole tilemap
            //when we hit it, we need to get the tile at the position of the hit?
            
            DestructableScript des = other.GetComponent<DestructableScript>();
            if(des.hittableByPlayer){
                //if the players current item's type matches the destructable script's type
                if(ItemManager.instance.currentItem.action_type == des.action_type) {
                    des.TakeDamage(ItemManager.instance.currentItem.damage);
                } else if (ItemManager.instance.currentItem) {
                    des.TakeDamage(ItemManager.instance.currentItem.damage * .2f);
                }   
            }
            
        }
    }
}
