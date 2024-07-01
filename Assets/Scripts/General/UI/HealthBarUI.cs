using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private IDamageable character;
    private float currentValue, totalValue = 10;
    [SerializeField] private Image healthImage;
    [SerializeField] private Transform characterTransform;

    private void Start()
    {
        currentValue = totalValue;
        character = characterTransform.GetComponent<IDamageable>();
        GameManager.Instance.OnGamePaused += Hide;
        GameManager.Instance.OnGameUnpaused += Show;
        character.OnHealthDepleted += OnCharacterHealthDepleted;
    }


    private void OnDestroy()
    {
        GameManager.Instance.OnGamePaused -= Hide;
        GameManager.Instance.OnGameUnpaused -= Show;
        character.OnHealthDepleted -= OnCharacterHealthDepleted;
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
    private void OnCharacterHealthDepleted(object sender, IDamageable.OnHealthDepletedEventArgs e)
    {
        //Update the values and fillAmount when character health depletes.
        this.currentValue = e.currentHealth;
        this.totalValue = e.totalHealth;
        healthImage.fillAmount = currentValue / totalValue;
    }
}
