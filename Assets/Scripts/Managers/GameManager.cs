using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles the overall game state, including pausing, resuming, and game over conditions.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float gameCounter { get; private set; } = 0f;
    public float gameLength { get; private set; } = 35f;

    public event Action OnGamePaused;
    public event Action OnGameUnpaused;
    public event Action OnGameOver;
    
    private bool GamePaused = false;
    private bool playerAlive = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        PauseGame();
        PlayerControls.Instance.OnPaused += PauseGame;
        PlayerController.OnPlayerDeath += PlayerController_OnPlayerDeath;
    }
    private void Update()
    {
        if (!GamePaused)
        {
            HandleGameTimer();
        }
    }

    private void OnDestroy()
    {
        PlayerControls.Instance.OnPaused -= PauseGame;
        PlayerController.OnPlayerDeath -= PlayerController_OnPlayerDeath;
    }

    private void HandleGameTimer()
    {
        gameCounter += Time.deltaTime;

        if (gameCounter >= gameLength || !playerAlive)
        {
            Time.timeScale = 0f;
            OnGameOver?.Invoke();
        }
    }
    private void PlayerController_OnPlayerDeath()
    {
        playerAlive = false;
    }

    private void PauseGame()
    {
        if (!GamePaused)
        {
            Time.timeScale = 0f;
            GamePaused = true;
            OnGamePaused?.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            GamePaused = false;
            OnGameUnpaused?.Invoke();
        }
    }
}
