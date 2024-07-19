using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentHealth : Health
{
    [SerializeField] private EnemyBehaviour behaviour;
    private bool isDead;

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0) Die();
    }
     
    public override void Die()
    {
        if(!isDead)
        {
            AudioManager.instance.PlayDeathAudio(transform);
            behaviour.Die();
            Destroy(gameObject, 3);
            isDead = true;
        }        
    }
   
}
