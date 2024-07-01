using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterHealthComponent 
{
    public int Health { get; set; }
    public int TotalHealth { get; private set; }

    public CharacterHealthComponent(int health)
    {
        this.Health = health;
        TotalHealth = health;
    }

    public bool IsHealthFullyDepleted() => Health <= 0;
    
}
