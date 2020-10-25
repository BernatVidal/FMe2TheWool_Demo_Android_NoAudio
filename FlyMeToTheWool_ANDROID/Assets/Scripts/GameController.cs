using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

[SerializeField]
public class GameController : MonoBehaviour
{

    [Header("Debris")]
    public DebrisManager spaceDebrisManager;
    public int debrisSpawnAmount = 10;
    public int debrisMinSpawnNumberGroup = 3;

    [Header("GameEntities")]
    public GameObject cat;
    private CatController catController;
    public GameObject dogUFO;
    public GameObject normalDrone;
    public GameObject bigDrone;

    [Header("PowerUps")]
    public float healthSpawnOddPercent = 2f;
    public float shieldSpawnOddPercent = 1.5f;
    public float tripleSpawnOddPercent = 0.5f;
    public GameObject healthPowerUp;
    public GameObject shieldPowerUp;
    public GameObject triplePowerUp;

    [Header("UI")]
    private int score = 0;
    public Text scoreText;
    private int lives = 7;
    public UI_LifeSystem lifeSystem;
    private Animator scoreAnim;

    [Header("GameOver Menu")]
    public GameOverMenu gOverMenu;


    [Header("Pause Menu")]
    public PauseMenu pauseMenuUI;
    public OptionsMenu opMenu;

    [Header("EndGameMenu")]
    public GameObject endGameMenuUI;
    private bool isGameEnded = false;
    public float timeBetweenEventsOnEndGame = 8f;
    public Text endGameScoreText;

    [Header("Tooltips")]
    public GameObject tooltipsUI;

    [Header("Audio")]
    AudioManager audioManager;
    public AudioMixer audioMixer;
    
    [HideInInspector]
    public CountDownScript timeCounter;

    private Transform nDroneEnemiesPool;


    private void Awake()
    {
        scoreAnim = scoreText.GetComponent<Animator>();

        catController = cat.GetComponent<CatController>();

        timeCounter = GetComponent<CountDownScript>();
        
        nDroneEnemiesPool = GameObject.FindWithTag("Pool_Enemies").transform;
    }

    private void Start()
    {
        gOverMenu.gameObject.SetActive(false);
        pauseMenuUI.gameObject.SetActive(false);
        endGameMenuUI.gameObject.SetActive(false);
        tooltipsUI.gameObject.SetActive(true);

        audioManager = AudioManager.instance;
        pauseMenuUI.audioMixer = audioMixer;

        score = 0;
        UpdateScore();
        lives = 7;
        UpdateLiveUI();
    }


    void Update()
    {
        if (!isGameEnded)
            PauseGameListener();

        // EndGame
        if (timeCounter.GetTimer() == 0 && !isGameEnded)
        {
            isGameEnded = true;
            StartCoroutine(EndGame(timeBetweenEventsOnEndGame));
            audioManager.PlaySound(Sounds.SoundID.EndGame);
        }

        ////// DEBUG  SPAWNS /////////
        ///
        //if (Input.GetKeyDown(KeyCode.N))
        //     spaceDebrisManager.SpawnObjects(debrisSpawnAmount);
        //  if (Input.GetKeyDown(KeyCode.M))
        //      StartCoroutine(SpawnDebris(5, 1f, 5));
        //  if (Input.GetKeyDown(KeyCode.K))
        //      StartCoroutine(SpawnNormalDrones(1, 5f, 5));
        //  if (Input.GetKeyDown(KeyCode.P))
        //      StartCoroutine(SpawnDogs(3, 5f, 5));
        //  if (Input.GetKeyDown(KeyCode.B))
        //      StartCoroutine(SpawnBigDrones(2, 0, 1));
        //////////////////////
        ///
        /// Pause and Options menu on Escape pressed
    }

    // Score System
    public void AddScore (int value)
    {
        score += value;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score : " + score;
        scoreAnim.Play("textShake");
    }


    // Life System
    public void SetLives(int newLives)
    {
        //Damaged
        if (newLives < lives)
        {
            audioManager.PlaySound(Sounds.SoundID.CatDamaged);
            catController.UnSetTripleShot();
        }
        //UpdateLives
        lives = newLives;
        UpdateLiveUI();
        //PlaySounds
        if (lives == 1)
            audioManager.PlaySound(Sounds.SoundID.CatOneLive);
        if (lives > 1 || lives ==0)
            audioManager.PauseSound(Sounds.SoundID.CatOneLive);
        //GameOver
        if (lives <= 0)
            GameOver();
    }
    private void UpdateLiveUI()
    {
        lifeSystem.UpdateLives(lives);
    }


    #region GAME STATUS
    private void GameOver()
    {
        audioManager.PlaySound(Sounds.SoundID.GameOver);
        gOverMenu.gameObject.SetActive(true);
        timeCounter.isGameOver = true;   //Stop Counter
    }


    void PauseGameListener()
    {
        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            if (pauseMenuUI.isGamePaused && !opMenu.isActiveAndEnabled)
            {
                audioManager.PlaySound(Sounds.SoundID.Pause);
                pauseMenuUI.Resume();
                pauseMenuUI.gameObject.SetActive(false);
            }
            else if (!pauseMenuUI.isGamePaused && !opMenu.isActiveAndEnabled)
            {
                audioManager.PlaySound(Sounds.SoundID.Pause);
                pauseMenuUI.gameObject.SetActive(true);
                pauseMenuUI.Pause();
            }
            else if (pauseMenuUI.isGamePaused && opMenu.isActiveAndEnabled)
            {
                audioManager.PlaySound(Sounds.SoundID.Pause);
                pauseMenuUI.gameObject.SetActive(true);
                opMenu.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator EndGame(float delayTime)
    {
        //Kill all enemies
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        for (var i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);

        gameObjects = GameObject.FindGameObjectsWithTag("Debris");
        for (var i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);

        //Make cat go forward
        catController.enabled = false;
        cat.GetComponent<PlayerLifeSystem>().enabled = false;
        cat.GetComponent<Rigidbody2D>().velocity = Vector3.up * 2;
        cat.transform.rotation = Quaternion.identity;

        //Set Score to Text
        endGameScoreText.text = "Your Score : " + score;

        yield return new WaitForSeconds(delayTime);
        //Show EndGameMenu
        endGameMenuUI.SetActive(true);
    }

    #endregion

    #region SPAWNERS
    //Game Events

    public IEnumerator SpawnDebris (int amountOfGroupsPerTime, float delayTime, int times)
    {

        for (int i = 0; i < times; i++)
        {
            for (int j = 0; j < amountOfGroupsPerTime; j++)
            {
                spaceDebrisManager.SpawnObjects(debrisMinSpawnNumberGroup);
                yield return new WaitForSeconds(delayTime/2);
            }
            yield return new WaitForSeconds(delayTime);
        }
    }



    public IEnumerator SpawnNormalDrones(int amountOfGroupsPerTime, float delayTime, int times)
    {
        Vector2 half = Utils.GetHalfDimensionsInWorldUnits();

        for (int i = 0; i < times; i++)
        {
            for (int j = 0; j < amountOfGroupsPerTime; j++)
            {
                Vector2 newPosition = new Vector2(Random.Range(-half.x + 0.3f, half.x + 0.3f), 5);
                var nDrone = Pool_NormalDrones.Instance.Get();
                nDrone.transform.parent = nDroneEnemiesPool;
                nDrone.transform.position = newPosition;
                nDrone.gameObject.SetActive(true);
                yield return new WaitForSeconds(delayTime/2);
            }
            yield return new WaitForSeconds(delayTime);
        }
    }

    public IEnumerator SpawnDogs (int amountOfGroupsPerTime, float delayTime, int times)
    {
        Vector2 half = Utils.GetHalfDimensionsInWorldUnits();

        for (int i = 0; i < times; i++)
        {
            for (int j = 0; j < amountOfGroupsPerTime; j++)
            {
                Vector2 newPosition = new Vector2(Random.Range(-half.x + 0.3f, half.x + 0.3f), 7);
                Instantiate(dogUFO, newPosition, Quaternion.identity);
                yield return new WaitForSeconds(delayTime/2);
            }
            yield return new WaitForSeconds(delayTime );
        }
    }

    public IEnumerator SpawnBigDrones(int amountOfGroupsPerTime, float delayTime, int times)
    {
        Vector2 half = Utils.GetHalfDimensionsInWorldUnits();

        for (int i = 0; i < times; i++)
        {
            for (int j = 0; j < amountOfGroupsPerTime; j++)
            {
                // Avoid spawning in same spots
                Vector2 newPosition = new Vector2(0,7);
                if (amountOfGroupsPerTime > 1) 
                {
                    if (j == 0)
                        newPosition = new Vector2(-half.x + 0.3f, 7);
                    if (j == 1)
                        newPosition = new Vector2(half.x + 0.3f, 7);
                }
                Instantiate(bigDrone, newPosition, Quaternion.identity);
                yield return new WaitForSeconds(delayTime / 2);
            }
            yield return new WaitForSeconds(delayTime);
        }
    }

    #endregion



}
