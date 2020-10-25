using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable_NormalDrones : GameEntity
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        //Don't be destroyed by particles with collisions or Debris
        if (!col.gameObject.CompareTag("Particles") && col.gameObject.layer != 13)
        {
            livesLost++;
            if (livesLost < totalLives)
                PlayHurtAnimation();
        }
        //Back to pool if looses all lives, collides with boundaries , with player (layer 8) 
        if (livesLost >= totalLives || col.gameObject.layer == 9 || col.gameObject.layer == 8 )
        {
            Animator anim = GetComponentInChildren<Animator>();
            anim.Play("Idle");
            livesLost = 0;
            Pool_NormalDrones.Instance.ReturnToPool(this);
        }

    }

    void PlayHurtAnimation()
    {
        Animator anim = GetComponentInChildren<Animator>();
        anim.Play("droneHurt");
    }
}
