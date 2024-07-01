using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    private SoundEffects soundEffects;

    private void Awake()
    {
        soundEffects = GetComponent<SoundEffects>();
    }

    private void Start()
    {
        BulletSpawnerComponent.OnBulletSpawned += PlayGunShotSound;
        PlayerController.OnDash += PlayDashSound;
        PlayerController.OnVault += PlayVaultSound;
        Enemy.OnAnyEnemyDeath += PlayEnemyDeathSound;
    }

    private void OnDestroy()
    {
        BulletSpawnerComponent.OnBulletSpawned -= PlayGunShotSound;
        PlayerController.OnDash -= PlayDashSound;
        PlayerController.OnVault -= PlayVaultSound;
        Enemy.OnAnyEnemyDeath -= PlayEnemyDeathSound;
    }

    private void PlayEnemyDeathSound(object sender, System.EventArgs e)
    {
        PlaySound(soundEffects.EnemyDeathSound, (sender as Enemy).transform.position);
    }

    private void PlayDashSound()
    {
        PlaySound(soundEffects.DashSound, PlayerController.Instance.transform.position);
    }

    private void PlayVaultSound()
    {
        PlaySound(soundEffects.VaultSound, PlayerController.Instance.transform.position);
    }

    private void PlayGunShotSound(object sender, System.EventArgs e)
    {
        PlaySound(soundEffects.ShootSound, (sender as BulletSpawnerComponent).transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector2 location)
    {
        AudioSource.PlayClipAtPoint(audioClip, location);
    }
}


