using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animationManager = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        //If movement is not 0, try not to move
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
        } else if (movementInput.y < 0 && movementInput.x == 0) {
            animationManager.SetBool("isMovingDown", true);
            animationManager.SetBool("isMovingUp", false);
        } 

        if(movementInput.x < 0) {
            spriteRenderer.flipX = true;

        } else if (movementInput.x > 0){
            spriteRenderer.flipX = false;

        }
    }

    private bool TryMove(Vector2 direction) {

            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if(count == 0) {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } 
            return false;

    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
}
