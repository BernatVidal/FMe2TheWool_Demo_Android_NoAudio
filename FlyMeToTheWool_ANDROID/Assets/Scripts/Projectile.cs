using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Projectile : GameEntity
{
    [Header("Projectile Settings")]
    public float shootSpeed = 0;
    public float directionAngle = 0;
    public float maxRotationSpeed = 0;
    public bool isRotationEnabled = true;


    protected Rigidbody2D rb;

    protected AudioManager audioManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        audioManager = AudioManager.instance;
    }

    public void ShootProjectile()
    {
        // Reset transform and rb to avoid using from pool previous object
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        livesLost = 0;

        // Set new rb settings
        rb.transform.Rotate(new Vector3(0, 0, directionAngle));
        rb.velocity = transform.up * shootSpeed;
        
        // Apply rotation to object if required
        if (isRotationEnabled)
            rb.angularVelocity = Random.Range(-maxRotationSpeed, maxRotationSpeed);
    }





}
