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
    private static SpriteRenderer swingableSR;
    private bool canMove = true;
    public swordScript swordAttack;
    private int direction = 2; 

    public Camera cam;
    public  GameObject swingable;

    public static ItemScriptableObject currentItem;
    private bool isCoroutineRunning;



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
        swingableSR = swingable.GetComponent<SpriteRenderer>();

        Transform swingableTransform = transform.Find("SwingableRotation");
        if(swingableTransform != null) {
            Transform actualSwingalbeTransform = swingableTransform.Find("SwingableItem");
            if (actualSwingalbeTransform != null) {
                swingableSR = actualSwingalbeTransform.GetComponent<SpriteRenderer>();
            }
        }
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
        if (swingableSR.sprite != currentItem.item_sprite) {
            swingableSR.sprite = currentItem.item_sprite;
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
        if(!IsPointerOverUI() && currentItem.swingable == true && isCoroutineRunning == false) {
            StartCoroutine(Swing());
        }
        
    }

    private IEnumerator Swing() {
        isCoroutineRunning = true;
        AudioManager.instance.PlayAtPosition("swordslash", transform.position, .5f, false);
        // Calculate the initial angle to point at the cursor
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;

        swingable.SetActive(true);
        // Set the sword active and initial rotation
        Quaternion initialRotation;
        Quaternion targetRotation;
        if(mousePos.x < transform.position.x) {
        //if(spriteRenderer.flipX == true) {
            
            swingable.transform.localRotation = Quaternion.Euler(0, 0, angle-135); // Start 45 degrees clockwise

            // Calculate the target rotation (90 degrees counterclockwise)
            initialRotation = swingable.transform.localRotation;
            targetRotation = Quaternion.Euler(0, 0, angle); // End 45 degrees counterclockwise
        } else {
            swingable.transform.localRotation = Quaternion.Euler(0, 0, angle+45); // Start 45 degrees clockwise

            // Calculate the target rotation (90 degrees counterclockwise)
            initialRotation = swingable.transform.localRotation;
            //print(angle - 45f);
            targetRotation = Quaternion.Euler(0, 0, angle - 90); // End 45 degrees counterclockwise
        }
        

        // Rotate the sword over time
        float elapsedTime = 0f;
        while (elapsedTime < currentItem.swingSpeed)
        {
            float t = elapsedTime / currentItem.swingSpeed;
            t = Mathf.SmoothStep(0, 1, t);
            swingable.transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        swingable.transform.localRotation = targetRotation; // Ensure final rotation is set

        // Deactivate the sword
        isCoroutineRunning = false;
        swingable.SetActive(false);
    }

    private bool IsPointerOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
                if (enemy != null) {
                    enemy.Health -= currentItem.damage;
                }
        } 
        else if (other.tag == "Destructable") {
            Debug.Log("Tree hit inside TRIGGER!");
            DestructableScript des = other.GetComponent<DestructableScript>();
            des.TakeDamage(currentItem.damage);
        }
    }
}
