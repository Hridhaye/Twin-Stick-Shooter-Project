using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handle the Game Over screen.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button restartButton;

    private int enemyDeathValue = 0;

    private void Start()
    {
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        GameManager.Instance.OnGameOver += ShowGameOverScreen;
        Enemy.OnAnyEnemyDeath += IncrementEnemyDeathValue;
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= ShowGameOverScreen;
        Enemy.OnAnyEnemyDeath -= IncrementEnemyDeathValue;
    }

    private void IncrementEnemyDeathValue(object sender, System.EventArgs e)
    {
        enemyDeathValue++;
    }

    private void ShowGameOverScreen()
    {
        recipesDeliveredText.text = enemyDeathValue.ToString();
        this.gameObject.SetActive(true);
    }



}
