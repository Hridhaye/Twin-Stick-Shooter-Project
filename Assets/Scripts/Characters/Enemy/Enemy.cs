using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controls the enemy's overall behavior, coordinating smaller, constituent classes.
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
{
    private const int TOTAL_HEALTH = 5;

    public static event EventHandler OnAnyEnemyDeath;
    public event EventHandler OnEnemyDeath;
    public event EventHandler<IDamageable.OnHealthDepletedEventArgs> OnHealthDepleted;

    [SerializeField] private Animator characterAnimator;
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private SpriteRenderer characterSpriteRenderer;

    private EnemyMovement enemyMovement;
    private Transform player;
    private EnemyAim enemyAim;
    private CharacterHealthComponent healthComponent;
    private CharacterVisualComponent visualComponent;

    private void Awake()
    {
        enemyAim = GetComponent<EnemyAim>();
        enemyMovement = GetComponent<EnemyMovement>();
        visualComponent = new CharacterVisualComponent(enemyMovement, characterAnimator, gunSpriteRenderer, characterSpriteRenderer);
        healthComponent = new CharacterHealthComponent(TOTAL_HEALTH);
    }

    private void Start()
    {
        player = PlayerController.Instance.transform;
    }

    private void Update()
    {
        CheckForDeath();

        enemyMovement.HandleMovement();
        visualComponent.SetRunningAnimationBool();

        enemyAim.HandleAiming(player.position);
        visualComponent.SetSpriteOrientation(enemyAim.AimAngle);

        ProximityLogic();
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
            OnAnyEnemyDeath?.Invoke(this, EventArgs.Empty);
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Handles logic based on the enemy's proximity to the player, including shooting and movement conditions.
    /// </summary>
    private void ProximityLogic()
    {
        if (enemyMovement.PlayerInsideShootRadius())
        {
            bool isShooting = enemyAim.TryShot();

            if (isShooting)
            {
                enemyMovement.ApplyMovementConditions();
            }
        }
    }
}
