using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// Manages the spawning of enemies using grid-based logic.
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    private const int X_SPAWN_UPPER_LIMIT = 9;
    private const int Y_SPAWN_UPPER_LIMIT = 5;

    public static EnemySpawnManager Instance { get; private set; }
    public List<Transform> enemies { get; private set; }

    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private GridClass gridClass;

    private Vector2 playerPosition;
    private float timer = 0f;
    private float spawnCounter = 1.5f;
    private int maxEnemiesOnScreen = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        enemies = new List<Transform>();
    }

    private void Update()
    {
        playerPosition = PlayerController.Instance.transform.position;

        if (timer < spawnCounter)
        {
            timer += Time.deltaTime;
        }
        else if (timer >= spawnCounter && enemies.Count < maxEnemiesOnScreen)
        {
            SpawnEnemy();
        }

    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    private void SpawnEnemy()
    {
        int playerXCoordinate = gridClass.GetNodeFromWorldPoint(playerPosition).GridX;
        int playerYCoordinate = gridClass.GetNodeFromWorldPoint(playerPosition).GridY;
        int spawnXCoordinate, spawnYCoordinate = 0;

        do
        {
            if (playerXCoordinate >= (gridClass.GridSizeX / 2))
            {
                spawnXCoordinate = GetSpawnCoordinate(playerXCoordinate, -1, X_SPAWN_UPPER_LIMIT);
            }
            else
            {
                spawnXCoordinate = GetSpawnCoordinate(playerXCoordinate, 1, X_SPAWN_UPPER_LIMIT);
            }

            if (playerYCoordinate >= (gridClass.GridSizeY / 2))
            {
                spawnYCoordinate = GetSpawnCoordinate(playerYCoordinate, -1, Y_SPAWN_UPPER_LIMIT);
            }
            else
            {
                spawnYCoordinate = GetSpawnCoordinate(playerYCoordinate, 1, Y_SPAWN_UPPER_LIMIT);
            }
        }
        while (!gridClass.Grid[spawnXCoordinate, spawnYCoordinate].IsWalkable);

        Vector2 spawnLocation = gridClass.Grid[spawnXCoordinate, spawnYCoordinate].nodeWorldPosition;

        Transform enemyTransform = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        enemy.OnEnemyDeath += EnemyDeathCleanUp;
        enemies.Add(enemy.transform);
    }
    
    private int GetSpawnCoordinate(int playerCoordinate, int direction, int offset)
    {
        int spawnCoordinate = UnityEngine.Random.Range(playerCoordinate + (1 * direction), playerCoordinate + (offset * direction));
        return spawnCoordinate;
    }

    private void EnemyDeathCleanUp(object sender, EventArgs e)
    {
        Enemy enemy = sender as Enemy;
        enemy.OnEnemyDeath -= EnemyDeathCleanUp;
        enemies.Remove(enemy.transform);
        ResetTimer();
    }


}
