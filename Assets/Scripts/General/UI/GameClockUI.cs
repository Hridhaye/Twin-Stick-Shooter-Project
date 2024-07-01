using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClockUI : MonoBehaviour
{
    [SerializeField] private Image clockImage;
    private void Start()
    {
        Show();
        GameManager.Instance.OnGameOver += Hide;
    }
    private void Update()
    {
        //The clock fills up relative to the game counter reaching its limit.
        clockImage.fillAmount = GameManager.Instance.gameCounter / GameManager.Instance.gameLength;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= Hide;
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }
}
