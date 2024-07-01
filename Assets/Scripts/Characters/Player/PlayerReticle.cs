using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the position of the reticle relative to the player based on mouse input.
/// </summary>
public class PlayerReticle : MonoBehaviour
{
    public Vector3 MouseOffsetFromPlayer { get; private set; }

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform reticleSprite;
    [SerializeField] private PlayerAim playerAim;

    private void Update()
    {
        Vector3 mousePosition = playerAim.LastMousePosition;
        MouseOffsetFromPlayer = mousePosition - playerTransform.position;

        float maxDistanceFromPlayer = 7f;
        if (MouseOffsetFromPlayer.magnitude > maxDistanceFromPlayer)
        {
            MouseOffsetFromPlayer = MouseOffsetFromPlayer.normalized * maxDistanceFromPlayer;
        }

        reticleSprite.position = playerTransform.position + MouseOffsetFromPlayer;
    }

}