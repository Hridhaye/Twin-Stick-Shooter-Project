using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles core bullet behavior regarding movement and collision.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayer;

    private float bulletSpeed = 1000f;
    private Vector2 bulletDirection;

    private Rigidbody2D bulletRB;
    private Collider2D bulletCollider;
    private Collider2D shooterCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnGetFromPool(Vector2 direction, Collider2D shooterCollider)
    {
        bulletDirection = direction;
        this.shooterCollider = shooterCollider;

        Physics2D.IgnoreCollision(bulletCollider, shooterCollider, true);

        bulletRB.velocity = bulletDirection * bulletSpeed * Time.fixedDeltaTime;

        if (shooterCollider.GetComponent<PlayerController>() != null)
        {
            spriteRenderer.color = Color.blue;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    public void OnReturnToPool()
    {
        bulletRB.velocity = Vector2.zero;

        if (shooterCollider != null)
        {
            Physics2D.IgnoreCollision(bulletCollider, shooterCollider, false);
        }

        shooterCollider = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == shooterCollider) return;

        // Check if collision layer matches.
        if (((1 << collision.gameObject.layer) & collisionLayer) != 0)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                damageable.DecrementHealth();
            }

            OnReturnToPool();
            BulletPool.Instance.ReturnBulletToPool(this);
        }
    }
}