﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable_WoolShotExplosion : MonoBehaviour
{
    //Used to pool explosion when finished

    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (!ps.IsAlive())
            Pool_WoolShotExplosions.Instance.ReturnToPool(this);
    }
}
