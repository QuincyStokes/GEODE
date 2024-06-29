using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Animator animationManager;
    SpriteRenderer spriteRenderer;
    private bool canMove = true;
    public swordScript swordAttack;
    private int direction = 2; 


    // 0 = right
    // 1 = left
    // 2 = down
    // 3 = up

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animationManager = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        //If movement is not 0, try not to move
        if(canMove) {
            if(movementInput != Vector2.zero) {
                bool success = TryMove(movementInput);
                if(!success) {
                    success = TryMove(new Vector2(movementInput.x, 0));

                    if(!success) {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                animationManager.SetBool("isMoving", success);
                animationManager.SetBool("isMovingUp", false);
                animationManager.SetBool("isMovingDown", false);
            } else { 
                animationManager.SetBool("isMoving", false);
                animationManager.SetBool("isMovingUp", false);
                animationManager.SetBool("isMovingDown", false);


            }
            if(movementInput.y > 0 && movementInput.x == 0) {
                animationManager.SetBool("isMovingUp", true);
                animationManager.SetBool("isMovingDown", false);
                direction = 3; //facing up
            } else if (movementInput.y < 0 && movementInput.x == 0) {
                animationManager.SetBool("isMovingDown", true);
                animationManager.SetBool("isMovingUp", false);
                direction = 2; //facing down
            } 

            if(movementInput.x < 0) {
                spriteRenderer.flipX = true;
                direction = 1;

            } else if (movementInput.x > 0){
                spriteRenderer.flipX = false;
                direction = 0;

            }
        } 
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

    void OnMove(InputValue movementValue) {
        canMove = true;
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire(){
        if(!IsPointerOverUI()) {
            animationManager.SetTrigger("player_attack");
            canMove = false;
            LockMovement();
            //need to implement swinging whatever item the player is holding (if its swingable)

            //First lets try getting the item sprite..
            //heldItem = playerInventory.playerInventory[playerInventory.selectedItem];
            //heldItemSprite.SpriteRenderer = playerInventory.selectedItemGameobject;

            //playerInventory[index].GetComponent<ItemPickable>().itemScriptableObject.item_sprite

        }
        
    }

    public void SwordAttack() {
        LockMovement();
        AudioManager.instance.Play("swordslash");
        switch(direction) {
            case 0:
                swordAttack.AttackRight();
                break;
            case 1:
                swordAttack.AttackLeft();
                break;
            case 2:
                swordAttack.AttackDown();
                break;
            case 3:
                swordAttack.AttackUp();
                break;

        }
       
    }

    public void StopAttack() {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement() {
        canMove = false;
        print("Movement Locked!");
    }

    public void UnlockMovement(){
        canMove = true;
        print("Movement Unlocked!");
    }

    private bool IsPointerOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
