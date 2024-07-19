using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int health;

    public abstract void TakeDamage(int damage);

    public abstract void Die();

    public int GetHealth()
    {
        return health;
    }
}
