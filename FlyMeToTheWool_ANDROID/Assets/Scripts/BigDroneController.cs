using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDroneController : GameEntity
{
    public float xMin, yMin, xMax, yMax;

    [Header("Movement")]
    public float horizontalAcceleration = 0.1f;
    public float verticalAcceleration = 0.1f;
    public float maxVelocity = 3f;
    public float minTimeDoingMoveAction = 1;
    public float maxTimeDoingMoveAction = 2;

    [Header("Shooting")]
    // public GameObject shot;
    public Transform shotOrigin;
    public float fireRate;
    private float nextFire = 0;
    public int shotsPerShoot = 3;
    public float timeBetweenShots = 0.3f;
    private float shotXScale = 1.5f;

    public int minDronesToShootAgain = 7;
    private bool isSpawningDrones = false;

    private Vector2 movementVector;
    private bool isCrMoveRunning = false;
    private bool isOutOfZone = false;

    private Animator anim;

    private bool isUp, isDown, isLeft, isRight;

    private bool isDroneShootingAvailable;

    private FlockManager flockManager;
    private Rigidbody2D rb;

    private Transform shootsPoolParent;

    AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movementVector = new Vector2(0, 0);

        flockManager = GetComponent<FlockManager>(); 
        anim = GetComponentInChildren<Animator>();

        shootsPoolParent = GameObject.FindWithTag("Pool_Shoots").transform;
    }

    private void Start()
    {
        UpdateLimits();
        audioManager = AudioManager.instance;
    }

    void UpdateLimits()
    {
        Vector2 half = Utils.GetHalfDimensionsInWorldUnits();
        xMin = -half.x;
        xMax = half.x;
        yMax = half.y;
    }

    void Update()
    {
        Move();

        if (!isSpawningDrones)
            CheckForShooting();

        if (transform.position.y < 4f)
            isDroneShootingAvailable = true;

        if (isDroneShootingAvailable && !isSpawningDrones)
            CheckDronesQuantity();

        //if (Input.GetKeyDown(KeyCode.L))
         //   ShootSmallDrones();
    }

    #region MOVEMENT
    private void Move()
    {
        if (IsOutOfComfortZone())
            GetBackToComfortZone();
        else if (!isCrMoveRunning)
        {
            StartCoroutine(MoveToRandomDirection(Random.Range(minTimeDoingMoveAction, maxTimeDoingMoveAction)));
        }


        if (isUp)
            GoUp();
        if (isDown)
            GoDown();
        if (isLeft)
            GoLeft();
        if (isRight)
            GoRight();

        if (isSpawningDrones)
            DontMove();


    }


    public bool IsOutOfComfortZone()
    {
        isOutOfZone = false;
        if (transform.position.x < xMin ||
                transform.position.x > xMax ||
                transform.position.y < yMin ||
                transform.position.y > yMax)
        {
            isOutOfZone = true;
        }

        return isOutOfZone;
    }

    private void GetBackToComfortZone()
    {
        if (transform.position.x < xMin)
        {
            isRight = true;
            isLeft = false;
        }
        if (transform.position.x > xMax)
        {
            isLeft = true;
            isRight = false;
        }
        if (transform.position.y < yMin)
        {
            isDown = true;
            isUp = false;
        }
        if (transform.position.y > yMax)
        {
            isUp = true;
            isDown = false;
        }
    }


    private void GoUp() { movementVector.y -= verticalAcceleration; }
    private void GoDown() { movementVector.y += verticalAcceleration; }
    private void GoLeft() { movementVector.x -= horizontalAcceleration; }
    private void GoRight() { movementVector.x += horizontalAcceleration; }

    private void DontMove() { movementVector.x = 0; movementVector.y = 0; }

    // Dice method to know new direction
    IEnumerator MoveToRandomDirection(float timeDoingAction)
    {
        isCrMoveRunning = true;

        int dice = Random.Range(1, 4);
        if (dice == 1)
        {
            isUp = true;
            isDown = false;
        }
        if (dice == 2)
        {
            isUp = false;
            isDown = true;
        }
        if (dice == 3)
        {
            isUp = false;
            isDown = false;
        }

        dice = Random.Range(1, 4);

        if (dice == 1)
        {
            isLeft = true;
            isRight = false;
        }
        if (dice == 2)
        {
            isLeft = false;
            isRight = true;
        }
        if (dice == 3)
        {
            isLeft = false;
            isRight = false;
        }

        yield return new WaitForSeconds(timeDoingAction);
        isCrMoveRunning = false;

    }

    #endregion


    #region SHOOTING

    void CheckForShooting()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate + Random.Range(2,5);
            StartCoroutine(ShootMultipleTimes());
        }
    }

    IEnumerator ShootMultipleTimes()
    {
        for (int i = 0; i < shotsPerShoot; i++)
        {
            Shoot();
            PlayShootAnim();
            yield return new WaitForSeconds(timeBetweenShots);
        }
        
    }

    private void Shoot()
    {
        // Get object from the pool, set world parameters and launch.
        var shot = Pool_DroneShots.Instance.Get();
        shot.transform.parent = shootsPoolParent;
        shot.transform.position = shotOrigin.position;
        shot.transform.rotation = shotOrigin.rotation;
        shot.gameObject.SetActive(true);
        shot.ShootProjectile();
        shot.transform.localScale = new Vector3(shot.transform.localScale.x * shotXScale, shot.transform.localScale.y, 1);
    }

    private void PlayShootAnim()
    {
        anim.Play("droneShoot");
    }


    #endregion

    #region DRONES
    void CheckDronesQuantity()
    {
        //Debug.Log(flockManager.GetQuantityOfActiveDrones());
        if (!isSpawningDrones &&
            flockManager.GetQuantityOfActiveDrones() <= minDronesToShootAgain)
        {
            // SHOOT - SPAWN SMALL DRONES
            StartCoroutine(SpawnSmallDrones());
            audioManager.PlaySound(Sounds.SoundID.SmallDroneSpawn);
        }
    }

    public IEnumerator SpawnSmallDrones()
    {
        isSpawningDrones = true;
        for (int i = 0; i < flockManager.startingCount; i++)
        {
            var drone = Pool_SmallDrones.Instance.Get();
            drone.transform.parent = flockManager.smallDronesPoolParent;
            drone.gameObject.SetActive(true);
            FlockAgent newAgent = drone.GetComponent<FlockAgent>();
            newAgent.transform.position = transform.position + Vector3.forward;
            newAgent.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(160, 200));
            flockManager.agents.Add(newAgent);

            yield return new WaitForSeconds(flockManager.timeBetweenSpawns);
        }
        isSpawningDrones = false;
    }


    #endregion

    #region COLLISIONS

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Particles") && col.gameObject.layer != 13)
        {
            livesLost++;
            if (livesLost < totalLives)
                PlayHurtAnimation();
        }
        //Back to pool if looses all lives, collides with boundaries , with player (layer 8) 
        if (livesLost >= totalLives || col.gameObject.CompareTag("Boundary") || col.gameObject.layer == 8)
        {
            audioManager.PauseSound(Sounds.SoundID.SmallDroneSpawn);
            Destroy(this.gameObject);
        }

    }

    void PlayHurtAnimation()
    {
        anim.Play("droneHurt");
    }
    #endregion

    private void FixedUpdate()
    {
        // Movement applying Lerping and Campling
        movementVector.x = Mathf.Clamp(movementVector.x, -maxVelocity, maxVelocity);
        movementVector.y = Mathf.Clamp(movementVector.y, -maxVelocity, maxVelocity);
        rb.velocity = movementVector;

    }

}
