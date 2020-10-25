using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable_DroneShot : Projectile
{
    //public float destroyParticlesScale = 0.5f;
    private void OnCollisionEnter2D(Collision2D col)
    {
        //Don't change on collision with Collidable Particles or Space Debris
        if (!col.gameObject.CompareTag("Particles") && col.gameObject.layer != 13) 
            livesLost++;
        if (livesLost >= totalLives)
        {
            if (col.gameObject.layer != 9)  //Boundaries
                PlayDestroyAnimation();
            Pool_DroneShots.Instance.ReturnToPool(this);
        }
    }

    private void PlayDestroyAnimation()
    {
        audioManager.PlaySound(Sounds.SoundID.DroneShotDestruction);
        var explosion = Pool_DroneShotExplosions.Instance.Get();
        explosion.transform.parent = GameObject.FindWithTag("Pool_Explosions").transform;
        explosion.transform.position = this.transform.position;
        //explosion.transform.localScale = new Vector3(destroyParticlesScale, destroyParticlesScale, 1);
        explosion.gameObject.SetActive(true);
    }


    void OnEnable()
    {
        audioManager.PlaySound(Sounds.SoundID.DroneShot);
    }

}

