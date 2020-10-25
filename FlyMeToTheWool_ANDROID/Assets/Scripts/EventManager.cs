using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EventManager : MonoBehaviour
{
    private GameController gc;
    private CountDownScript counter;
    private int[] eventTimes;
    [SerializeField]
    private int eventNum = 0; //Change this number to test each event
    public int totalNumOfEvents = 14;

    private void Awake()
    {
        gc = GetComponent<GameController>();
        counter = GetComponent<CountDownScript>();
    }

    private void Start()
    {
        SetEventTimes();
    }

    private void Update()
    {
        if (counter.GetTimer() < eventTimes[eventNum])
        {
            LaunchEvent();
            Debug.Log("Launched Event " + eventNum);
            eventNum++;
        }
    }

    void LaunchEvent()
    {
        switch (eventNum)
        {
            case 0:
                gc.tooltipsUI.SetActive(false);
                StartCoroutine(gc.SpawnDebris(5, 1.5f, 5));
                break;
            case 1:
                StartCoroutine(gc.SpawnNormalDrones(2, 5f, 2));
                break;
            case 2:
                StartCoroutine(gc.SpawnDebris(5, 1f, 6));
                break;
            case 3:
                StartCoroutine(gc.SpawnDebris(5, 1f, 2));
                StartCoroutine(gc.SpawnNormalDrones(3, 3f, 2));
                break;
            case 4:
                StartCoroutine(gc.SpawnDogs(1, 3f, 3));
                break;
            case 5:
                StartCoroutine(gc.SpawnDogs(2, 4f, 2));
                StartCoroutine(gc.SpawnNormalDrones(2, 3f, 2));
                break;
            case 6:
                StartCoroutine(gc.SpawnDebris(4, 1.2f, 5));
                break;
            case 7:
                StartCoroutine(gc.SpawnBigDrones(1, 0, 1));
                break;
            case 8:
                StartCoroutine(gc.SpawnDebris(5, 1f, 3));
                StartCoroutine(gc.SpawnNormalDrones(3, 3f, 3));
                break;
            case 9:
                StartCoroutine(gc.SpawnDogs(2, 3f, 2));
                StartCoroutine(gc.SpawnDebris(4, 1.2f, 5));
                break;
            case 10:
                StartCoroutine(gc.SpawnDogs(1, 3.2f, 3));
                StartCoroutine(gc.SpawnNormalDrones(3, 3f, 2));
                break;
            case 11:
                StartCoroutine(gc.SpawnDebris(5, 2f, 3));
                break;
            case 12:
                StartCoroutine(gc.SpawnDogs(1, 2f, 1));
                StartCoroutine(gc.SpawnNormalDrones(2, 2f, 2));
                break;
            case 13:
                StartCoroutine(gc.SpawnBigDrones(2, 1, 1));
                break;
            case 14:
                StartCoroutine(gc.SpawnDebris(5, 0.5f, 2));
                break;


        }
    }

    private void SetEventTimes()
    {
        eventTimes = new int[totalNumOfEvents + 1];
        eventTimes[0] = 293;
        eventTimes[1] = 270;
        eventTimes[2] = 250;
        eventTimes[3] = 235;
        eventTimes[4] = 220;
        eventTimes[5] = 195;
        eventTimes[6] = 155;
        eventTimes[7] = 135;
        eventTimes[8] = 115;
        eventTimes[9] =  95;
        eventTimes[10]=  80;
        eventTimes[11]=  65;
        eventTimes[12] = 55;
        eventTimes[13] = 35;
        eventTimes[14] = 15;


    }
    /////////////////////////////////////////
    /*

            EVENTS :
    Time   Event                  |Sec|timer-sec|Ev num 
    0:10 Debris                      7     293     0
    0:30 Normal Drones              30     270     1
    0:50 Debris                     50     250     2
    1:05 Debris + Normal Drones     65     235     3
    1:17 Dogs                       77     220     4
    1:45 Dogs + normal drones       105    195     5
    2:25 Debris                     145    155     6
    2:45 Big Drone                  165    135     7
    3:05 Debris + NDrones           185    115     8
    3:25 Debris + Dogs              205     95     9
    3:40 Normal Drones + dogs       220     80    10
    3:55 Debris                     235     65    11
    4:05 NDrones + Dogs             245     55    12
    4:25 2 Big Drones               265     35    13
    4:45 Debris                     285     15    14
   
    */

    /////////////////////////////////////////
}
