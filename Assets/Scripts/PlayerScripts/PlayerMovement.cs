using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private bool canMove = true;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private Vector2 movementX = Vector2.zero;
    private Vector2 movementY = Vector2.zero;

    void FixedUpdate()
    {   
        if (canMove && movementInput != Vector2.zero)
        {
            float deltaMoveSpeed = moveSpeed * Time.fixedDeltaTime;
            bool success = TryMove(movementInput, deltaMoveSpeed);
            if (!success)
            {
                movementX.Set(movementInput.x, 0);
                success = TryMove(movementX, deltaMoveSpeed);
                if (!success)
                {
                    movementY.Set(0, movementInput.y);
                    success = TryMove(movementY, deltaMoveSpeed);
                }
            }
        }
    }

    private bool TryMove(Vector2 direction, float deltaMoveSpeed)
    {
        int count = rb.Cast(direction, movementFilter, castCollisions, deltaMoveSpeed + collisionOffset);
        if (count == 0)
        {
            print("Cast hit nothing.");
            rb.MovePosition(rb.position + direction * deltaMoveSpeed);
            return true;
        } else 
        {
             print("Cast hit something.");
        }
       
        return false;
    }

     public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }

    public Vector2 GetMovementInput()
    {
        return movementInput;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
