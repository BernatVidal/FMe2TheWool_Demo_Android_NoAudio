using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class PowerUpBehaviour : MonoBehaviour
{
    public float velocity = 1f;
    private float randomVelDifference = 0.2f;
    public string id;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = Vector3.down * (Random.Range(velocity - randomVelDifference, velocity + randomVelDifference));
    }

    private void OnEnable()
    {
        AudioManager.instance.PlaySound(Sounds.SoundID.PowerUpAppear);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        AudioManager.instance.PlaySound(Sounds.SoundID.PowerUpObtained);
        Destroy(gameObject);
    }

}
