using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerMovement))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private int direction = 2; // 0 = right, 1 = left, 2 = down, 3 = up

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    //create vector2 to hold previous input, only change animation if we're moving in a new direction.
    private Vector2 lastMovementInput = Vector2.zero;

    void Update()
    {
        Vector2 movementInput = playerMovement.GetMovementInput();
        bool isMoving = movementInput != Vector2.zero;

        animator.SetBool("isMoving", isMoving);

        if (movementInput != lastMovementInput)
        {
            if (isMoving)
            {
                UpdateDirection(movementInput);
            }
            lastMovementInput = movementInput;
        }
    }

    void UpdateDirection(Vector2 movementInput)
    {
        animator.SetBool("isMovingUp", false);
        animator.SetBool("isMovingDown", false);


        if (movementInput.y > 0 && movementInput.x == 0)
        {
            animator.SetBool("isMovingUp", true);
            animator.SetBool("isMovingDown", false);
            direction = 3;
        }
        else if (movementInput.y < 0 && movementInput.x == 0)
        {
            animator.SetBool("isMovingDown", true);
            animator.SetBool("isMovingUp", false);
            direction = 2;
        }

        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
            direction = 1;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
            direction = 0;
        }
    }

    public int GetDirection()
    {
        return direction;
    }
}