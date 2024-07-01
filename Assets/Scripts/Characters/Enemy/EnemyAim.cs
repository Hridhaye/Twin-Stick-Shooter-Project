using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour, IAimable
{
    private const float BULLET_SPAWN_COOLDOWN = 0.3f;

    public float AimAngle { get; private set; } = 0f;

    [SerializeField] private Transform aimPoint;
    [SerializeField] private float enemyAimSpeed;
    [SerializeField] private LayerMask shotObstacleLayer;

    private Vector2 playerPosition;
    private CharacterAimingComponent aimingComponent;
    private BulletSpawnerComponent bulletSpawner;

    private void Awake()
    {
        aimingComponent = new CharacterAimingComponent();
        bulletSpawner = GetComponent<BulletSpawnerComponent>();
        bulletSpawner.ChangeBulletSpawnCooldown(BULLET_SPAWN_COOLDOWN);
    }

    public void HandleAiming(Vector2 targetPosition)
    {
        this.playerPosition = targetPosition;
        AimAngle = aimingComponent.GetAimAngle(playerPosition, aimPoint.position, enemyAimSpeed * Time.deltaTime);
        aimPoint.eulerAngles = new Vector3(0f, 0f, AimAngle);
    }

    public bool TryShot()
    {
        if (aimingComponent.HasClearShot(playerPosition, aimPoint.position, shotObstacleLayer))
        {
            bulletSpawner.SpawnBullet();
            return true;
        }
        else
        {
            return false;
        }
    }
}
