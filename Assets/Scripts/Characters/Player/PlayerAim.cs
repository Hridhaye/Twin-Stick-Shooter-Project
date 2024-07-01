using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates the aim point toward the mouse's world position, thereby handling aiming.
/// </summary>
public class PlayerAim : MonoBehaviour, IAimable
{
    public Vector2 LastMousePosition { get; private set; } = Vector2.right;
    public float AimAngle { get; private set; } = 0f;

    [SerializeField] private Transform aimPoint;
    [SerializeField] private Camera mainCamera;

    private Vector2 mousePosition;
    private CharacterAimingComponent aimingComponent;

    private void Awake()
    {
        aimingComponent = new CharacterAimingComponent();
    }

    public void HandleAiming(Vector2 mouseScreenPosition)
    {
        mousePosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        if (mousePosition != LastMousePosition && mouseScreenPosition != Vector2.zero)
        {
            LastMousePosition = mousePosition;
        }

        AimAngle = aimingComponent.GetAimAngle(LastMousePosition, transform.position);
        aimPoint.eulerAngles = new Vector3(0f, 0f, AimAngle);
    }

}
