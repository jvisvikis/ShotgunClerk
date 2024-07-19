using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public override void TakeDamage(int damage)
    {
        health -= damage;
        // GameManager.instance.DamageTaken();
        if(health <= 0) Die();
    }

    public override void Die()
    {
        // GameManager.instance.GameOver(false);
    }
}
