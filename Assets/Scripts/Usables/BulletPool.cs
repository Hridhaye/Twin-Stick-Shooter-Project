using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    [SerializeField] private Bullet bulletPrefab;

    private int initialPoolSize = 20;
    private ObjectPool<Bullet> bulletPool;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        bulletPool = new ObjectPool<Bullet>(bulletPrefab, initialPoolSize);
    }

    public Bullet GetBullet()
    {
        Bullet bullet = bulletPool.Get();
        return bullet;
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        bulletPool.ReturnToPool(bullet);
    }
}
