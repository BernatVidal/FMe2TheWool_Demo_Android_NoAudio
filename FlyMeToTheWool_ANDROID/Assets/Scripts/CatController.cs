
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[System.Serializable]
public class Limits
{
    public float xMin, xMax, yMin, yMax;
}

public class CatController : MonoBehaviour
{
    [Header("Movement")]
    public float horizontalVelocity = 4.0f;
    public float verticalVelocity = 3.0f;
    public float tiltAmount;
    public float tiltVel;
    private float tiltMax = 10f;
    public Limits limits;
    private bool isMoveSoundPlaying = false;

    [Header("Shooting")]
    public Transform shotOrigin;
    public float fireRate;
    public float recoilForce;
    private float nextFire;
    private Transform poolShotParent;

    [Header("PowerUps")]
    public bool isShield = false;
    private float shieldTime = 10f;
    public GameObject shieldPS;
    private float timeShielded = 0f;
    public UI_ShieldSystem shieldUI;
    private bool isTriple = false;
    public GameObject tripleUIgObj;

    public ParticleSystem flames;
    private float flamesSpeedVariation = 0.8f;
    private Rigidbody2D rb;
    private Vector2 movementVector;

    AudioManager audioManager;

    private Animator anim;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movementVector = new Vector2(0, 0);

        poolShotParent = GameObject.FindWithTag("Pool_Shoots").transform;

        anim = GetComponentInChildren<Animator>();

        shieldUI.gameObject.SetActive(false);
        shieldTime = shieldUI.shieldTime;
        tripleUIgObj.SetActive(false);
    }

    private void Start()
    {
        UpdateLimits();
        isShield = false;
        isTriple = false;

        audioManager = AudioManager.instance;
    }

    void UpdateLimits()
    {
        Vector2 half = Utils.GetHalfDimensionsInWorldUnits();
        limits.xMin = -half.x - 0.2f;
        limits.xMax = half.x + 0.2f;
        limits.yMin = -half.y;
    }


    void Update()
    {
        Move();
        CheckForShooting();
        CheckForShield();
    }

    private void Move()
    {    
        movementVector.x = CrossPlatformInputManager.GetAxis("Horizontal");
        movementVector.y = CrossPlatformInputManager.GetAxis("Vertical");

        PlayMoveSound();

        //Particle system feedback   // Deprecated
        flames.startSpeed = 1 + movementVector.y * flamesSpeedVariation;

    }

    private void PlayMoveSound()
    {
        
        if(movementVector.x != 0 || movementVector.y !=0)
        {
            if (!isMoveSoundPlaying)
            {
                isMoveSoundPlaying = true;
                audioManager.PlaySound(Sounds.SoundID.CatMove);
            }
        }
        else
        {
            isMoveSoundPlaying = false;
            audioManager.PauseSound(Sounds.SoundID.CatMove);
        }
    }

    #region SHOOTING
    private void CheckForShooting()
    {

        if (Time.time > nextFire && Time.timeScale !=0)
        {
            if (CrossPlatformInputManager.GetButton("Jump"))
            {
                Shoot();
                PlayShootAnim();
                audioManager.PlaySound(Sounds.SoundID.CatShot);
                //Recoil();
            }
        }
          
    }

    private void Shoot()
    {
        
        nextFire = Time.time + fireRate;
        //Instantiate(shot, shotOrigin.position, shotOrigin.rotation); 

        // Get object from the pool, set world parameters and launch.

        var shot = Pool_WoolShots.Instance.Get();
        shot.transform.parent = poolShotParent;
        shot.transform.position = shotOrigin.position;
        shot.transform.rotation = shotOrigin.rotation;
        shot.directionAngle = 0;
        shot.gameObject.SetActive(true);
        shot.ShootProjectile();

        if (isTriple)
        {
            shot = Pool_WoolShots.Instance.Get();
            shot.transform.parent = poolShotParent;
            shot.transform.position = shotOrigin.position;
            shot.transform.rotation = shotOrigin.rotation;
            shot.directionAngle = -25 - tiltAmount;
            shot.gameObject.SetActive(true);
            shot.ShootProjectile();

            shot = Pool_WoolShots.Instance.Get();
            shot.transform.parent = poolShotParent;
            shot.transform.position = shotOrigin.position;
            shot.transform.rotation = shotOrigin.rotation;
            shot.directionAngle = 25 + tiltAmount;
            shot.gameObject.SetActive(true);
            shot.ShootProjectile();
        }
        
    }

    private void PlayShootAnim()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("hurt"))
        {
            anim.Play("catShoot");
        }
    }

    private void Recoil()
    {
       // rb.AddForce(Vector2.down * recoilForce);
    }

    #endregion

    #region POWERUPS

    void CheckForShield()
    {
        if (isShield)
        {
            if (Time.time < timeShielded)
                shieldUI.UpdateShieldbar(timeShielded - Time.time, shieldTime);
            else
                UnsetShield();
        }
    }

    public void SetShield()
    {
        timeShielded = Time.time + shieldTime;
        shieldUI.gameObject.SetActive(true);
        isShield = true;
        shieldPS.SetActive(true);
    }

    private void UnsetShield()
    {
        shieldUI.gameObject.SetActive(false);
        isShield = false;
        shieldPS.SetActive(false);
    }

    public void SetTripleShot()
    {
        isTriple = true;
        tripleUIgObj.SetActive(true);
    }

    public void UnSetTripleShot()
    {
        isTriple = false;
        tripleUIgObj.SetActive(false);
    }

#endregion

    private void FixedUpdate()
    {
        // Movement
        movementVector.x = Mathf.Lerp(rb.velocity.x, movementVector.x * horizontalVelocity, tiltVel); //* Time.deltaTime*25);
        movementVector.y *= verticalVelocity; //* Time.deltaTime * 25;
        rb.velocity = movementVector;
        //Boundaries
        if (rb.simulated)
            rb.position = new Vector2(Mathf.Clamp(rb.position.x, limits.xMin, limits.xMax), Mathf.Clamp(rb.position.y, limits.yMin, limits.yMax));

      //Rotation
       transform.rotation =  Quaternion.Euler(0f, 0f,Mathf.Clamp(movementVector.x * -tiltAmount, -tiltMax, tiltMax));
    }
}
