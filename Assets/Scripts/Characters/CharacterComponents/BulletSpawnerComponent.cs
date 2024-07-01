using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the bullet spawning for characters.
/// </summary>
public class BulletSpawnerComponent : MonoBehaviour
{
    public static event EventHandler OnBulletSpawned;

    [SerializeField] private Transform FirePoint;

    private float spawnCounter = 0f;
    private float spawnCooldownDuration = 0.15f;

    private void Update()
    {
        if (spawnCounter < spawnCooldownDuration)
        {
            spawnCounter += Time.deltaTime;
        }
    }

    public void ChangeBulletSpawnCooldown(float spawnCooldown)
    {
        this.spawnCooldownDuration = spawnCooldown;
    }

    public void SpawnBullet()
    {
        if (spawnCounter >= spawnCooldownDuration)
        {
            Vector2 firepointDirection = FirePoint.right.normalized;

            Bullet bullet = BulletPool.Instance.GetBullet();
            Collider2D shooterCollider = FirePoint.parent.GetComponent<Collider2D>();

            bullet.transform.position = FirePoint.position;
            bullet.transform.rotation = FirePoint.rotation;

            //The bullet class will set the bullet's direction to firepointDirection and turn off collisions with shooterCollider
            bullet.OnGetFromPool(firepointDirection, shooterCollider);

            spawnCounter = 0f;
            OnBulletSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
}