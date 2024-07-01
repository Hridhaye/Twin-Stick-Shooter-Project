using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the Pause screen.
/// </summary>
public class PauseUI : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnGamePaused += GamePaused;
        GameManager.Instance.OnGameUnpaused += GameUnpaused;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGamePaused -= GamePaused;
        GameManager.Instance.OnGameUnpaused -= GameUnpaused;
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void GamePaused()
    {
        Show();
    }
    private void GameUnpaused()
    {
        Hide();
    }
}
