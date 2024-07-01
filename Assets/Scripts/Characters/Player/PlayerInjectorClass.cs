using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initializes and injects dependencies for various player components.
/// </summary>
public class PlayerInjectorClass : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Collider2D playerCollider;
    private PlayerControls playerControls;
    private PlayerController playerController;

    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private PlayerVault playerVault;
    private PlayerAim playerAim;
    private BulletSpawnerComponent bulletSpawner;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerDash = GetComponent<PlayerDash>();
        playerVault = GetComponent<PlayerVault>();
        playerAim = GetComponent<PlayerAim>();
        bulletSpawner = GetComponent<BulletSpawnerComponent>();
        playerControls = PlayerControls.Instance;

        playerController.Initialize(playerMovement, playerDash, playerVault, playerControls, playerAim, bulletSpawner);
        playerMovement.Initialize(playerRB);
        playerDash.Initialize(playerRB);
        playerVault.Initialize(playerRB, playerCollider);

    }


}
