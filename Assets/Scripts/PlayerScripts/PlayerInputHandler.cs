using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAttack))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementInput = movementValue.Get<Vector2>();
        playerMovement.SetMovementInput(movementInput);
    }

    void OnFire()
    {
        print("Firing!");
        playerAttack.Attack();
    }
}