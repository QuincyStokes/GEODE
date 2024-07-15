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
        if(swingableSR && currentItem) {
            if (swingableSR.sprite != currentItem.item_sprite) {
                swingableSR.sprite = currentItem.item_sprite;
                print("Changing item to " + currentItem.item_type);
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
        if(currentItem) {
            if(!IsPointerOverUI() && currentItem.swingable == true && isCoroutineRunning == false) {
                StartCoroutine(Swing());
            }
        }           
    }

    private IEnumerator Swing() {
        isCoroutineRunning = true;
        AudioManager.instance.Play("swordslash", .5f, false);
        // Calculate the initial angle to point at the cursor
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //direction that the pointer is at? could be wrong
                                //arctan(distance from the pointer to the player) * Rad2Deg (this just changes from radian to degree)
                                //whole thing gives the angle from flat to where the mouse is from
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //Vector3 relative = transform.InverseTransformPoint(mousePos);
        //float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        //float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        swingable.SetActive(true);
        // Set the sword active and initial rotation
        Quaternion initialRotation;
        Quaternion targetRotation;
        if(mousePos.x < transform.position.x) {
        //if(spriteRenderer.flipX == true) {
                                                                    //subtract by the whole angle we want
            swingable.transform.localRotation = Quaternion.Euler(0, 0, angle-currentItem.swingRadius); // Start 45 degrees clockwise
            initialRotation = swingable.transform.localRotation;
            targetRotation = Quaternion.Euler(0, 0, angle);
        } else {
            swingable.transform.localRotation = Quaternion.Euler(0, 0, angle+currentItem.swingRadius/3); // Start 45 degrees clockwise
            
            initialRotation = swingable.transform.localRotation;
            targetRotation = Quaternion.Euler(0, 0, angle - (currentItem.swingRadius/3) * 2); // End 45 degrees counterclockwise
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

        elapsedTime = 0f;

        while(Input.GetButton("Fire1")) {
            //swingable.transform.localRotation = targetRotation;
            
            //swingable.transform.localRotation = targetRotation; // Ensure final rotation is set
            elapsedTime = 0f;
            while (elapsedTime < currentItem.swingSpeed) {
                float t = elapsedTime / currentItem.swingSpeed;
                t = Mathf.SmoothStep(0, 1, t);
                swingable.transform.localRotation = Quaternion.Lerp(targetRotation, initialRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
           
            elapsedTime = 0f;
            while (elapsedTime < currentItem.swingSpeed) {
                float t = elapsedTime / currentItem.swingSpeed;
                t = Mathf.SmoothStep(0, 1, t);
                swingable.transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

        }
        // Deactivate the sword
        isCoroutineRunning = false;
        swingable.SetActive(false);
    }

    private bool IsPointerOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        /*
        if(other.tag == "Enemy") {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
                if (enemy != null) {
                    enemy.Health -= currentItem.damage;
                }
        } 
        */
        /* dont think we need this anymore.
        else if (other.tag == "Destructable") {
            //"other" is probably the whole tilemap
            //when we hit it, we need to get the tile at the position of the hit?

            DestructableScript des = other.GetComponent<DestructableScript>();
            if(currentItem) {
                des.TakeDamage(currentItem.damage);
            }
        }*/
    }
}
