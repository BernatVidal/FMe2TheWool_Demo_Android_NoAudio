using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CountDownScript : MonoBehaviour
{
    public Text uiText;
    public float mainTimer;

    private float timer;
    private bool canCount = true;
    public bool isGameOver;


    private void Start()
    {
        timer = mainTimer;
    }

    void Update ()
    {
        if (isGameOver) 
        { 
        }
        else if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;

            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = Mathf.Floor(timer % 60).ToString("00");

            uiText.text = minutes + ":" + seconds;
        }

        else if (timer <= 0.0f && canCount)
        {
            canCount = false;
            uiText.text = "00:00";
            timer = 0.0f;
        }

    }

    public float GetTimer()
    {
        return timer;
    }

}
