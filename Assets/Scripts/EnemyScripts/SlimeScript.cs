using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    Animator animationManager;
    public AudioSource slimeJump; 

    public void Start() {
        animationManager = GetComponent<Animator>();
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
}
