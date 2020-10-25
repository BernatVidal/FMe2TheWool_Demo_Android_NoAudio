using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Avoid errors on editor when stop game
public class Deactivator : MonoBehaviour
{
    void OnApplicationQuit()
    {
        MonoBehaviour[] scripts = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }
}