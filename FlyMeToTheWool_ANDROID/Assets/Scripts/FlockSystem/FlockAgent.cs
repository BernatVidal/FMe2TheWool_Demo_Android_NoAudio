using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    public int numOfSprites = 3;

    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        GetRandomSprite();
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void GetRandomSprite()
    {
        SpriteRenderer[]  sprites = GetComponentsInChildren<SpriteRenderer>();
        int dice = Random.Range(0,3);
        for (int i = 0; i < numOfSprites; i++)
        {
            if (i == dice)
                sprites[i].enabled = true;
            else sprites[i].enabled = false;
        }
        
    }
}
