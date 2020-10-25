using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Holder : MonoBehaviour
{


    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Health")
            SetHealth();
        if (col.gameObject.tag == "Shield")
            SetShield();
        if (col.gameObject.tag == "Triple")
            SetTriple();
    }


    void SetHealth()
    {
        GetComponent<PlayerLifeSystem>().AddLive();
    }

    void SetShield()
    {
        GetComponent<CatController>().SetShield();
    }

    void SetTriple()
    {
        GetComponent<CatController>().SetTripleShot();
    }
}
