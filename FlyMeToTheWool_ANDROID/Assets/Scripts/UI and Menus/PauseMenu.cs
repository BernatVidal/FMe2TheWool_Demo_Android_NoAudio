using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public bool isGamePaused = false;

    [HideInInspector]
    public AudioMixer audioMixer;
    private void Start()
    {
        isGamePaused = true;

    }

    //Pause Menu
    public void Pause()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        audioMixer.SetFloat("Master", -80);
    }


    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        audioMixer.SetFloat("Master", 0);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

