using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipExplodesOnDestroy : MonoBehaviour
{
    public float explosionScale = 1f;

    private void OnDisable()
    {
            PlayDestroyAnimation();
    }

    private void PlayDestroyAnimation()
    {
        var explosion = Pool_SpaceshipExplosions.Instance.Get();
        
        if (this.transform.position != Vector3.zero)
        {         
            explosion.transform.position = transform.position;
            AudioManager.instance.PlaySound(Sounds.SoundID.SpaceshipDestruction);
        }
        else   //Avoid UnPooling weirdness
            explosion.transform.position = new Vector3(4, 8, 0);

        explosion.transform.parent = GameObject.FindWithTag("Pool_Explosions").transform;
        explosion.transform.localScale = new Vector3(explosionScale, explosionScale, 1);
        explosion.gameObject.SetActive(true);
    }
}
