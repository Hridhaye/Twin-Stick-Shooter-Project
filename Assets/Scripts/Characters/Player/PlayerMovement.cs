using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the player's movement, including smoothing and velocity management.
/// </summary>
public class PlayerMovement : MonoBehaviour, IMovable
{
    public bool isRunning { get; private set; }
    public Vector2 lastMovementVector { get; private set; }

    [SerializeField] private float moveSpeed;
    private float smoothingTime = 0.05f;

    private Vector2 smoothedMovementVector;
    private Vector2 smoothedVelocityRef;
    private Rigidbody2D playerRB;


    public void Initialize(Rigidbody2D playerRB)
    {
        this.playerRB = playerRB;
    }

    public void UpdateMovementInput(Vector2 movementVector, bool isDashing, bool isVaulting)
    {
        smoothedMovementVector = Vector2.SmoothDamp(smoothedMovementVector, movementVector, ref smoothedVelocityRef, smoothingTime);

        if (lastMovementVector != movementVector && movementVector != Vector2.zero)
        {
            lastMovementVector = movementVector;
        }

        isRunning = movementVector != Vector2.zero && !isDashing && !isVaulting;
    }

    public void HandleMovement()
    {
        playerRB.velocity = smoothedMovementVector * moveSpeed * Time.fixedDeltaTime;
    }


}
