using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; }

    public event Action OnDash;
    public event Action OnShoot;
    public event EventHandler OnInteract;
    public event Action OnPaused;

    private Controls controls;
    private Vector2 movementVector;
    private float movementDelay = 0.02f;
    private float nextMovementUpdate = 0f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        controls = new Controls();
        controls.Enable();
    }

    private void Start()
    {
        controls.Player.Dash.performed += Dash_performed;
        controls.Player.Interact.performed += Interact_performed;
        controls.Player.Shoot.performed += Shoot_performed;
        controls.Player.Pause.performed += Pause_performed;
    }

    private void OnDisable()
    {
        controls.Player.Dash.performed -= Dash_performed;
        controls.Player.Interact.performed -= Interact_performed;
        controls.Player.Shoot.performed -= Shoot_performed;
        controls.Player.Pause.performed -= Pause_performed;
        controls.Disable();
    }

    public Vector2 GetMouseScreenPosition()
    {
        return controls.Player.MousePosition.ReadValue<Vector2>();
    }

    public Vector2 GetMovementVector()
    {
        //Buffer between movement vector updates ensures smoothness.
        if (Time.time >= nextMovementUpdate)
        {
            movementVector = controls.Player.Move.ReadValue<Vector2>();
            nextMovementUpdate = Time.time + movementDelay;
        }

        return movementVector;
    }


    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPaused?.Invoke();
    }

    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        OnShoot?.Invoke();
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        OnDash?.Invoke();
    }

    
}
