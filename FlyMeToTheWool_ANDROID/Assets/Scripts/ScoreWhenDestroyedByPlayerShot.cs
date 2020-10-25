using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreWhenDestroyedByPlayerShot : MonoBehaviour
{
    public GameEntity entity;
    public int points;


    private GameController gc;


    private void Awake()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // PlayerShot = layer 12
        if (col.gameObject.layer == 12 && entity.totalLives == entity.livesLost + 1)
        {
            //Debug.Log("Add Score " + points);
            gc.AddScore(points);
            GetPowerUp(gc.healthSpawnOddPercent, gc.shieldSpawnOddPercent, gc.tripleSpawnOddPercent);
        }
    }


    void GetPowerUp(float healthProbability, float shieldProbability, float tripleProbability)
    {

        float dice = Random.Range(0, 100);
        if (dice <= healthProbability )
        {
            Instantiate(gc.healthPowerUp, transform.position + Vector3.back, Quaternion.identity);
        }
        else if (dice < healthProbability + shieldProbability)
        {
            Instantiate(gc.shieldPowerUp, transform.position + Vector3.back, Quaternion.identity);
        }
        else if ( gc.timeCounter.GetTimer() < 195 &&
            dice < healthProbability + shieldProbability + tripleProbability
            )
        {
            Instantiate(gc.triplePowerUp, transform.position + Vector3.back, Quaternion.identity);
        }
    }

}
