using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimable 
{
    public float AimAngle { get; }
    void HandleAiming(Vector2 targetPosition);
}
