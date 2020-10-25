using UnityEngine;

public class Poolable_Debris3 : Poolable_DebrisClass
{
    public float destroyParticlesScale = 0.8f;
    private void OnCollisionEnter2D(Collision2D col)
    {
        //Don't be destroyed by particles with collisions, Enemies
        if (!col.gameObject.CompareTag("Particles") || col.gameObject.layer != 10)
            livesLost++;

        //Back to pool if looses all lives, collides with boundaries , with player (layer 8) , with DroneShot  
        if (livesLost >= totalLives || col.gameObject.layer == 9 || col.gameObject.layer == 8 || col.gameObject.CompareTag("DroneShot"))
        {
            if (col.gameObject.layer != 9)  //Boundaries 
                PlayDestroyAnimation(destroyParticlesScale);
            livesLost = 0;
            Pool_Debris3.Instance.ReturnToPool(this);          
        }

    }

}

