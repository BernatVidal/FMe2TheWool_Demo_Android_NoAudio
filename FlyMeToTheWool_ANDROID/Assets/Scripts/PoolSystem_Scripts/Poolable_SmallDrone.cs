using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable_SmallDrone : GameEntity
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Particles") && col.gameObject.layer != 13)
            livesLost++;
        if (livesLost >= totalLives)
        {
            livesLost = 0;
            Pool_SmallDrones.Instance.ReturnToPool(this);
        }
    }

}
