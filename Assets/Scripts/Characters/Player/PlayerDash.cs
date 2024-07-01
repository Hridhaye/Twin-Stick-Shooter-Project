using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's ability to dash and double-dash.
/// </summary>
public class PlayerDash : MonoBehaviour
{
    private const int DASHES_ALLOWED = 2;

    public bool IsDashing { get; private set; }
    public bool BegunDash { get; private set; }

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashCooldownDuration = 0.6f;

    private float dashCounter = 1f;
    private int dashesPerformed = 0;
    private Rigidbody2D playerRB;
    private Vector2 playerLastMovementVector;
    private bool CanDash;


    public void Initialize(Rigidbody2D playerRB)
    {
        this.playerRB = playerRB;
    }

    public void UpdateDashCounter()
    {
        if (dashCounter < dashCooldownDuration)
        {
            dashCounter += Time.deltaTime;
        }
        else
        {
            CanDash = true;
            dashesPerformed = 0;
        }
    }

    public void TryDash(Vector2 playerLastMovementVector)
    {
        this.playerLastMovementVector = playerLastMovementVector;

        if (CanDash && dashesPerformed < DASHES_ALLOWED)
        {
            IsDashing = true;
            BegunDash = true;
            dashCounter = 0f;
            dashesPerformed++;
        }
    }

    public void HandleDash()
    {
        playerRB.velocity = playerLastMovementVector * dashSpeed * Time.fixedDeltaTime;
        BegunDash = false;

        if (dashCounter >= dashDuration)
        {
            IsDashing = false;
            CanDash = false;
            dashCounter = 0f;
        }
    }


}
