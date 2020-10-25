
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class DogUFOController : GameEntity
{

    public float xMin, yMin, xMax, yMax;

    [Header("Movement")]
    public float horizontalAcceleration = 0.1f;
    public float verticalAcceleration = 0.1f;
    public float maxVelocity = 3f;
    public float tiltAmount;
    public float tiltVel;
    public float minTimeDoingMoveAction = 1;
    public float maxTimeDoingMoveAction = 2;

    [Header("Shooting")]
    // public GameObject shot;
    public Transform shotOrigin;
    public float fireRate;
    private float nextFire = 0;
    public int shotsPerShoot = 3;
    public float timeBetweenShots = 0.3f;
    public Transform objectiveToAim;


    private Vector2 movementVector;
    private bool isCrRunning = false;
    private bool isOutOfZone = false;

    private Animator anim;

    private bool isUp, isDown, isLeft, isRight;

    private Transform shootPoolParent;


    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movementVector = new Vector2(0, 0);

        anim = GetComponentInChildren<Animator>();

        shootPoolParent = GameObject.FindWithTag("Pool_Shoots").transform;
    }

    private void Start()
    {
        UpdateLimits();
    }

    void UpdateLimits()
    {
        Vector2 half = Utils.GetHalfDimensionsInWorldUnits();
        xMin = -half.x;
        xMax = half.x;
        //limits.yMin = -half.y;
        yMax = half.y;
    }

    void Update()
    {
        Move();
    }

    #region MOVEMENT
    private void Move()
    {
        if (IsOutOfComfortZone())
            GetBackToComfortZone();
        else if (!isCrRunning)
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

    }


    public bool IsOutOfComfortZone()
    {
        isOutOfZone = false;
        if (    transform.position.x < xMin ||
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


    private void GoUp()     { movementVector.y -= verticalAcceleration; }
    private void GoDown()   { movementVector.y += verticalAcceleration; }
    private void GoLeft()   { movementVector.x -= horizontalAcceleration; }
    private void GoRight()  { movementVector.x += horizontalAcceleration; }

    // Dice method to know new direction
    IEnumerator MoveToRandomDirection(float timeDoingAction)
    {
        isCrRunning = true;
        
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

        yield return new WaitForSeconds (timeDoingAction);
        isCrRunning = false;

    }

    #endregion


    #region SHOOTING

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Time.time > nextFire)
        {
            if (col.gameObject.layer == 8) //Player
            {
                nextFire = Time.time + fireRate;
                StartCoroutine(ShootMultipleTimes());
            }
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
        var shot = Pool_DogShots.Instance.Get();
        shot.transform.parent = shootPoolParent;
        shot.transform.position = shotOrigin.position;
        shot.transform.rotation = shotOrigin.rotation;
        shot.gameObject.SetActive(true);
        shot.ShootProjectile();
    }

    private void PlayShootAnim()
    {
        anim.Play("dogShoot");
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
            Destroy(this.gameObject);
        }

    }

    void PlayHurtAnimation()
    {
        anim.Play("dogHurt");
    }
    #endregion

    private void FixedUpdate()
    {
        // Movement applying Lerping and Campling
        movementVector.x = Mathf.Lerp(rb.velocity.x, movementVector.x * horizontalAcceleration, tiltVel);
        movementVector.x = Mathf.Clamp(movementVector.x, -maxVelocity, maxVelocity);
        movementVector.y = Mathf.Lerp(rb.velocity.y, movementVector.y * verticalAcceleration, tiltVel);
        movementVector.y = Mathf.Clamp(movementVector.y, -maxVelocity, maxVelocity);
        rb.velocity = movementVector;


        //Rotation
        transform.rotation = Quaternion.Euler(0f, 0f, rb.velocity.x * -tiltAmount);
    }




}
