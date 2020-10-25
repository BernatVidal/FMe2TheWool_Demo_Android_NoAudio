using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Poolable_DebrisClass : Projectile
{

    public void PlayDestroyAnimation(float scale)
    {
        audioManager.PlaySound(Sounds.SoundID.DebrisDestruction);
        var explosion = Pool_DebrisExplosions.Instance.Get();
        explosion.transform.parent = GameObject.FindWithTag("Pool_Explosions").transform;
        explosion.transform.position = this.transform.position;
        explosion.transform.localScale = new Vector3(scale, scale, scale);
        explosion.gameObject.SetActive(true);
    }
}
