using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingableScript : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
                if (enemy != null) {
                    enemy.Health -= PlayerController.currentItem.damage;
                }
        } 
        else if (other.tag == "Destructable") {
            //"other" is probably the whole tilemap
            //when we hit it, we need to get the tile at the position of the hit?
            
            DestructableScript des = other.GetComponent<DestructableScript>();
            if(des.hittableByPlayer){
                //if the players current item's type matches the destructable script's type
                if(PlayerController.currentItem.action_type == des.action_type) {
                    des.TakeDamage(PlayerController.currentItem.damage);
                } else if (PlayerController.currentItem) {
                    des.TakeDamage(PlayerController.currentItem.damage * .2f);
                }   
            }
            
        }
    }
}
