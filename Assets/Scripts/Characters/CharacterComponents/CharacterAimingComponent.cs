using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the aiming calculations for characters.
/// </summary>
public class CharacterAimingComponent
{
    public Vector2 AimDirection { get; private set; }

    private float lerpedAimAngle = 0f;

    public float GetAimAngle(Vector2 target, Vector2 start, float lerpTime = 0f)
    {
        AimDirection = (target - start).normalized;
        float aimAngle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;

        //Only enemy characters will have a lerpTime to ensure their aim isn't instantaneous and perfect. 
        //Player will have no lerpTime to ensure smooth control.
        if (lerpTime == 0f)
        {
            return aimAngle;
        }
        else
        {
            lerpedAimAngle = Mathf.LerpAngle(lerpedAimAngle, aimAngle, lerpTime);
            return lerpedAimAngle;
        }
    }

    public bool HasClearShot(Vector2 target, Vector2 start, LayerMask obstacleLayer)
    {
        float distance = Vector2.Distance(target, start);
        return !Physics2D.Raycast(start, AimDirection, distance, obstacleLayer);
    }
}
