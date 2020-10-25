using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeSystem : GameEntity
{

    private Animator anim;
    public GameController gc;
    private CatController catController;

    public float timeOfUnvulnerability = 1f;
    public float nextVulnerability = 0f;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        catController = gc.cat.GetComponent<CatController>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    { 
        if (col.gameObject.layer != 15 && !catController.isShield)  //15 = PowerUps
        {
            if (Time.time > nextVulnerability)
            {
                if (!col.gameObject.CompareTag("Particles"))
                {
                    livesLost++;
                    anim.Play("catHurt");
                    gc.SetLives(totalLives - livesLost);
                    nextVulnerability = Time.time + timeOfUnvulnerability;
                }
                if (livesLost >= totalLives)
                {
                    this.gameObject.SetActive(false);
                    //GameOver
                }
            }
        }
    }


    public void AddLive()
    {
        if (livesLost > 0 )
        {
            livesLost--;
            gc.SetLives(totalLives - livesLost);
        }
    }

}
