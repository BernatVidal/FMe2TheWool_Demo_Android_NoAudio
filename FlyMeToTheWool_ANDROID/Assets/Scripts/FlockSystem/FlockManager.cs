using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public FlockAgent agentPrefab;
    [HideInInspector]
    public List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(10, 80)]
    public int startingCount = 30;
   // const float AGENT_DENSITY = 0.08f;
    public float timeBetweenSpawns = 0.2f;
    //private Transform droneSpawnPoint;

    [Range(1f, 30f)]
    public float driveFactor = 10;
    [Range(1f, 30f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    [HideInInspector]
    public Transform smallDronesPoolParent;

    void Awake()
    {
        smallDronesPoolParent = GameObject.FindWithTag("Pool_Enemies").transform;
    }

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

    }


    private void Update()
    {
        foreach (FlockAgent agent in agents)
        { 
            List<Transform> context = GetNearbyObjects(agent);

            Vector2 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)  //Using sqrMagnitude take care
            {
                move = move.normalized * maxSpeed;  //Clamp velocity
            }
            agent.Move(move);
        }
    }


    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;

    }


    public int GetQuantityOfActiveDrones()
    {
        int quantity = 0;
        foreach (FlockAgent agent in agents)
        {
            if (agent.isActiveAndEnabled)
                quantity++;
        }
        return quantity;
    }


    //Destory all small drones when destroyed. May cause errors on editor buit not ingame
    private void OnDestroy()
    {
        foreach (FlockAgent agent in agents)
        { 
            if (agent.isActiveAndEnabled)
                Pool_SmallDrones.Instance.ReturnToPool(agent.GetComponent<Poolable_SmallDrone>());
        }
        agents.Clear();
    }
}

