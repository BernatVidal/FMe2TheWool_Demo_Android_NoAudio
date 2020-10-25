using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class NormalDroneController : MonoBehaviour
{
    [Header("Vertical Movement Parameters")]
    public float verticalNormalVelocity = 1.5f;
    public float verticalSlowVelocity = 0.2f;
    public float verticalVelocityRandomVariation = 0.5f;
    private bool isGoingSlow = false;

    public float timeBetweenVerticalVelChanges = 3f;
    public float randomAmountBetweenVerticalVelChanges = 1f;
    private bool isVerticalSpeedChangeAvailable = true;

    //[Header("Horizontal Movement Parameters")]
    private bool isOutOfZone = false;
    private bool moveHorizontal = true;
    private bool moveRight = false;
    private bool isHorizontalMoveChangeAvailable = true;
    public float xMin, xMax;

    [Header("Shooting Parameters")]
    public Transform shotOrigin;
    public float timeBetweenShots = 1.5f;
    public float randomTimeBetweenShooting = 0.8f;
    public int shotsPerShoot = 2;
    private bool isShootingAvailable = true;
    public float shotXScale = 0.8f;


    private Transform shootsPoolParent;

    private Animator anim;

    AudioManager audioManager;

    private Rigidbody2D rb;
    private Vector3 movementVector;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.down * Random.Range(verticalNormalVelocity - verticalVelocityRandomVariation, verticalNormalVelocity + verticalVelocityRandomVariation);
        anim = GetComponentInChildren<Animator>();

        shootsPoolParent = GameObject.FindWithTag("Pool_Shoots").transform;
    }

    private void OnEnable()
    {
        anim.Play("Idle");
        isVerticalSpeedChangeAvailable = true;
        isHorizontalMoveChangeAvailable = true;
        moveHorizontal = true;
        isShootingAvailable = true;
    }

    private void Start()
    {
        UpdateLimits();

        anim.Play("Idle");

        audioManager = AudioManager.instance;
    }

    void UpdateLimits()
    {
        Vector2 half = Utils.GetHalfDimensionsInWorldUnits();
        xMin = -half.x;
        xMax = half.x;
    }

    void Update()
    {
        Move();
        CheckForShooting();
    }

    #region MOVEMENT
    private void Move()
    {
        VerticalMovement();
        HorizontalMovement();
    }


    #region VERTICAL MOVE
    // Vertical Movement
    private void VerticalMovement()
    {
        if (isVerticalSpeedChangeAvailable)
            StartCoroutine(VerticalVelocityChanger());
    }

    IEnumerator VerticalVelocityChanger()
    {
        isVerticalSpeedChangeAvailable = false;
        if (isGoingSlow)
        {
            isGoingSlow = false;
            movementVector.y = - Random.Range(verticalSlowVelocity - verticalVelocityRandomVariation, verticalSlowVelocity + verticalVelocityRandomVariation);
        }
        else {
            isGoingSlow = true;
            movementVector.y = - Random.Range(verticalNormalVelocity - verticalVelocityRandomVariation, verticalNormalVelocity + verticalVelocityRandomVariation);
        }

        yield return new WaitForSeconds(Random.Range(timeBetweenVerticalVelChanges - randomAmountBetweenVerticalVelChanges,
                                                      timeBetweenVerticalVelChanges + randomAmountBetweenVerticalVelChanges));
        isVerticalSpeedChangeAvailable = true;
    }

    #endregion

    #region HORIZONTAL MOVE

    //HorizontalMovement
    private void HorizontalMovement()
    {
        if (IsOutOfComfortZone())
        {
            if (isOutOfZone)
                GetBackToComfortZone();
        }
        else if (isHorizontalMoveChangeAvailable)
            StartCoroutine(ChangeHorizontalMove());
    }

    public bool IsOutOfComfortZone()
    {
        isOutOfZone = false;
        if (transform.position.x < xMin || transform.position.x > xMax)
            isOutOfZone = true;

        return isOutOfZone;
    }

    private void GetBackToComfortZone()
    {
        moveHorizontal = true;
        if (transform.position.x < xMin)
            moveRight = true;
        if (transform.position.x > xMax)
            moveRight = false;

        ChangeMove();
    }

    IEnumerator ChangeHorizontalMove()
    {
        isHorizontalMoveChangeAvailable = false;

        // Move or not move
        int dice = Random.Range(0, 2);
        if (dice == 0)
            moveHorizontal = false;
        if (dice == 1)
            moveHorizontal = true;

        // If Move, right or left
        if (moveHorizontal)
        {
            dice = Random.Range(0, 2);
            if (dice == 0)
                moveRight = false;
            if (dice == 1)
                moveRight = true;
        }
        ChangeMove();
        yield return new WaitForSeconds(Random.Range(timeBetweenVerticalVelChanges / 2 - randomAmountBetweenVerticalVelChanges, timeBetweenVerticalVelChanges / 2 + randomAmountBetweenVerticalVelChanges));

        isHorizontalMoveChangeAvailable = true;
    }

    private void ChangeMove()
    {
        if (!moveHorizontal)  //Stop Horizontal move
            movementVector.x = 0;
        else if (moveRight)  // Go right
            movementVector.x = Random.Range(verticalNormalVelocity - verticalVelocityRandomVariation, verticalNormalVelocity + verticalVelocityRandomVariation);
        else    // Go left
            movementVector.x = - Random.Range(verticalNormalVelocity - verticalVelocityRandomVariation, verticalNormalVelocity + verticalVelocityRandomVariation);
    }

    #endregion

    #endregion

    #region SHOOTING
    private void CheckForShooting()
    {
        if (isShootingAvailable)
            StartCoroutine(ShootMultipleTimes());
    }

    IEnumerator ShootMultipleTimes()
    {
        isShootingAvailable = false;
        for (int i = 0; i < shotsPerShoot; i++)
        {
            PlayShootAnim();
            Shoot();
            yield return new WaitForSeconds(Random.Range(timeBetweenShots - randomTimeBetweenShooting, timeBetweenShots + randomTimeBetweenShooting));
        }
        yield return new WaitForSeconds(Random.Range(timeBetweenShots - randomTimeBetweenShooting, timeBetweenShots + randomTimeBetweenShooting));
        isShootingAvailable = true;
    }

    void Shoot()
    {
        //Poolable_DroneShot droneShot;
        var shot = Pool_DroneShots.Instance.Get();
        shot.transform.parent = shootsPoolParent;
        shot.transform.position = shotOrigin.position;
        shot.transform.rotation = shotOrigin.rotation;
        shot.gameObject.SetActive(true);
        shot.ShootProjectile();
        shot.transform.localScale = new Vector3(shot.transform.localScale.x * shotXScale, shot.transform.localScale.y, 1);
    }

    void PlayShootAnim()
    {
        anim.Play("droneShoot");
    }

    #endregion


    private void FixedUpdate()
    {

        rb.velocity = movementVector;
    }


}
