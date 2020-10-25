using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable_DogShot : Projectile
{

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Particles") )
            livesLost++;
        if (livesLost >= totalLives )
        {
            if(col.gameObject.layer != 9)  //Boundaries
                PlayDestroyAnimation();
            Pool_DogShots.Instance.ReturnToPool(this);
        }
    }

    private void PlayDestroyAnimation()
    {
        audioManager.PlaySound(Sounds.SoundID.DogShotDestruction);
        var explosion = Pool_DogShotExplosions.Instance.Get();
        explosion.transform.parent = GameObject.FindWithTag("Pool_Explosions").transform;
        explosion.transform.position = this.transform.position;
        explosion.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        audioManager.PlaySound(Sounds.SoundID.DogShot);
    }

}
