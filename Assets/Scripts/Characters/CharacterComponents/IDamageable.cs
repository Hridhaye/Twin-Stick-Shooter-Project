using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public event EventHandler<OnHealthDepletedEventArgs> OnHealthDepleted;
    public class OnHealthDepletedEventArgs : EventArgs
    {
        public int currentHealth;
        public int totalHealth;
    }

    void DecrementHealth();
    void CheckForDeath();
}
