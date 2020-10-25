using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable_DroneShotExplosion : MonoBehaviour
{
    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!ps.IsAlive())
            Pool_DroneShotExplosions.Instance.ReturnToPool(this);
    }
}
