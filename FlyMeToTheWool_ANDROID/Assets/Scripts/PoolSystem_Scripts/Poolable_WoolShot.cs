using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;


public class Poolable_WoolShot : Projectile
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Particles"))
            livesLost++;
        if (livesLost >= totalLives )

        {
            if (col.gameObject.layer != 9)  //Boundaries
                PlayDestroyAnimation();
            Pool_WoolShots.Instance.ReturnToPool(this);
        }

        if (col.gameObject.CompareTag("Enemy"))
        {
            audioManager.PlaySound(Sounds.SoundID.EnemyHurt);
            audioManager.PlaySound(Sounds.SoundID.EnemyHurt2);
        }
    }

    private void PlayDestroyAnimation()
    {
        audioManager.PlaySound(Sounds.SoundID.WoolExplosion);
        var explosion = Pool_WoolShotExplosions.Instance.Get();
        explosion.transform.parent = GameObject.FindWithTag("Pool_Explosions").transform;
        explosion.transform.position = this.transform.position;
        explosion.gameObject.SetActive(true);
    }



}
