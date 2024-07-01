using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controls the player's overall behavior, coordinating smaller, constituent classes.
/// </summary>
public class PlayerController : MonoBehaviour, IDamageable
{
    private const int TOTAL_HEALTH = 10;

    public static PlayerController Instance { get; private set; }

    public static event Action OnDash;
    public static event Action OnVault;
    public static event Action OnPlayerDeath;
    public event EventHandler<IDamageable.OnHealthDepletedEventArgs> OnHealthDepleted;

    [SerializeField] private Transform gunSprite;
    [SerializeField] private Transform characterSprite;

    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private PlayerVault playerVault;
    private PlayerControls playerControls;
    private PlayerAim playerAim;
    private BulletSpawnerComponent bulletSpawner;

    private CharacterVisualComponent visualComponent;
    private CharacterHealthComponent healthComponent;


    /// <summary>
    /// Awake is a Unity method that is called even before the Start method.
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Start is a Unity method that is called once before the first frame.
    /// </summary>
    private void Start()
    {
        playerControls.OnDash += DashInput;
        playerControls.OnInteract += InteractInput;
        playerControls.OnShoot += ShootInput;

        visualComponent = new CharacterVisualComponent(playerMovement, characterSprite.GetComponent<Animator>(),
                          gunSprite.GetComponent<SpriteRenderer>(), characterSprite.GetComponent<SpriteRenderer>());
        healthComponent = new CharacterHealthComponent(TOTAL_HEALTH);
    }

    /// <summary>
    /// Update is a Unity method that is called once per frame.
    /// </summary>
    private void Update()
    {
        CheckForDeath();

        playerMovement.UpdateMovementInput(playerControls.GetMovementVector(), playerDash.IsDashing, playerVault.IsVaulting);
        visualComponent.SetRunningAnimationBool();

        playerAim.HandleAiming(playerControls.GetMouseScreenPosition());
        visualComponent.SetSpriteOrientation(playerAim.AimAngle);

        playerDash.UpdateDashCounter();
        playerVault.CheckForVault(playerMovement.lastMovementVector);

        HandleSoundEffectEvents();
    }

    /// <summary>
    /// FixedUpdate is a Unity method that is called on a reliable, consistent interval, regardless of the frame rate. 
    /// It is used for physics calculations and updates, as it runs in sync with the physics engine.
    /// </summary>
    private void FixedUpdate()
    {
        if (playerVault.IsVaulting)
        {
            playerVault.HandleVault();
        }
        else if (playerDash.IsDashing)
        {
            playerDash.HandleDash();
        }
        else
        {
            playerMovement.HandleMovement();
        }
    }

    /// <summary>
    /// OnDestroy is a Unity method that is run when a GameObject is destroyed (such as when a scene ends).
    /// </summary>
    private void OnDestroy()
    {
        playerControls.OnDash -= DashInput;
        playerControls.OnInteract -= InteractInput;
        playerControls.OnShoot -= ShootInput;
    }



    public void Initialize(PlayerMovement playerMovement, PlayerDash playerDash, PlayerVault playerVault, PlayerControls playerControls,
        PlayerAim playerAim, BulletSpawnerComponent bulletSpawner)
    {
        this.playerMovement = playerMovement;
        this.playerDash = playerDash;
        this.playerVault = playerVault;
        this.playerControls = playerControls;
        this.playerAim = playerAim;
        this.bulletSpawner = bulletSpawner;
    }

    public void IncrementHealth()
    {
        healthComponent.Health++;
    }

    public void DecrementHealth()
    {
        healthComponent.Health--;

        //Signal to HealthBarUI to update health bar.
        OnHealthDepleted?.Invoke(this, new IDamageable.OnHealthDepletedEventArgs
        {
            currentHealth = healthComponent.Health,
            totalHealth = healthComponent.TotalHealth,
        });
    }

    public void CheckForDeath()
    {
        //Signal to GameManager to end game if player health is fully depleted.
        if (healthComponent.IsHealthFullyDepleted())
        {
            OnPlayerDeath?.Invoke();
        }
    }

    private void HandleSoundEffectEvents()
    {
        //Signal to SoundManager to play appropriate sounds.
        if (playerDash.BegunDash)
        {
            OnDash?.Invoke();
        }

        if (playerVault.BegunVault)
        {
            OnVault?.Invoke();
        }
    }
    


    private void InteractInput(object sender, EventArgs e)
    {
        playerVault.TryVault();
    }

    private void DashInput()
    {
        playerDash.TryDash(playerMovement.lastMovementVector);
    }

    private void ShootInput()
    {
        bulletSpawner.SpawnBullet();
    }


}
