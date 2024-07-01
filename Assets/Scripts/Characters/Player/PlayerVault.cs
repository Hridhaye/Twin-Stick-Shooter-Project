using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's vaulting ability, including detection and execution; this class will be refactored as more interaction functionality is added.
/// </summary>
public class PlayerVault : MonoBehaviour
{
    private const float OFFSET = 0.3f;
    private const float SMALL_SELECTION_RAY = 2f;
    private const float LONG_SELECTION_RAY = 4.5f;

    public bool IsVaulting { get; private set; } = false;
    public bool BegunVault { get; set; }

    [SerializeField] private float vaultSpeed;
    [SerializeField] private float vaultObjectWidth;
    [SerializeField] private float vaultObjectLength;
    [SerializeField] private LayerMask vaultableLayerMask;

    private RaycastHit2D vaultObject;
    private Vector2 vaultObjectHitPoint;
    private Vector2 vaultObjectNormal;
    private Vector2 vaultLandingPosition;
    private float interactSelectionDistance;
    private bool interactingInputReceived = false;

    private Rigidbody2D playerRB;
    private Collider2D playerCollider;


    public void Initialize(Rigidbody2D playerRB, Collider2D playerCollider)
    {
        this.playerRB = playerRB;
        this.playerCollider = playerCollider;
    }
    public void TryVault()
    {
        if (!interactingInputReceived)
        {
            interactingInputReceived = true;

            if (vaultObject)
            {
                vaultObjectHitPoint = vaultObject.point;
                vaultObjectNormal = vaultObject.normal;
                BegunVault = true;
            }
        }
    }

    public void CheckForVault(Vector2 playerLastMovementVector)
    {
        interactSelectionDistance = Mathf.Abs(playerRB.velocity.magnitude) < 3f ? SMALL_SELECTION_RAY : LONG_SELECTION_RAY;
        vaultObject = Physics2D.Raycast(transform.position, playerLastMovementVector, interactSelectionDistance, vaultableLayerMask);

        if (interactingInputReceived)
        {
            if (vaultObject && !IsVaulting)
            {
                playerRB.isKinematic = true;
                playerCollider.enabled = false;

                //If the dot product of the player's movement vector and the object's right axis is zero or near-zero, then the two are perpendicular.
                //So, the player is approaching the object's top or bottom and would need to traverse the object's length in order to scale it.
                //If the dot product is greater than near-zero, then the player is likely approaching the object from its left or right side. 
                //Edge cases are yet to be handled.
                float dotProduct = Vector2.Dot(playerLastMovementVector, vaultObject.transform.right);
                const float epsilon = 1e-5f;
                float magnitude = Mathf.Abs(dotProduct) < epsilon ? vaultObjectLength : vaultObjectWidth;

                //Ensure the player lands in front of the object, not on top of it.
                magnitude += OFFSET; 

                //Increment the landing position by half of the player's size because the player’s transform is positioned at their center. 
                //Note: the player's x and y scale values are the same.
                magnitude += (transform.localScale.y) / 2;

                vaultLandingPosition = vaultObjectHitPoint + (-vaultObjectNormal.normalized * magnitude);
                IsVaulting = true;
            }
        }

        if (interactingInputReceived && !vaultObject)
        {
            interactingInputReceived = false;
        }
    }


    public void HandleVault()
    {
        transform.position = Vector2.MoveTowards(transform.position, vaultLandingPosition, vaultSpeed * Time.fixedDeltaTime);
        BegunVault = false;

        if (Vector2.Distance(transform.position, vaultLandingPosition) < 0.1f)
        {
            playerRB.isKinematic = false;
            playerCollider.enabled = true;
            interactingInputReceived = false;
            IsVaulting = false;
        }
    }



}
