
using UnityEngine;


public class DebrisManager : MonoBehaviour
{
    public GameObject[] spaceDebris;
    public float[] oddsOfEachPercentAdditive;

    public Transform spawnPoint;

    private Transform debrisPoolParent;

    public int spawnAmount = 40;
    public float randomSpawnRadius = 3f;
    public float minVel, maxVel;
    public float minAngle, maxAngle;


    private void Awake()
    {
        debrisPoolParent = GameObject.FindWithTag("Pool_Debris").transform;

    }

    public void SpawnObjects(int amountOfDebris)
    {
        for (int i = 0; i < amountOfDebris; i++)
        {
            // Calculate new spawnpoint based on circle
            Vector3 circleSpawnRandom = UnityEngine.Random.insideUnitCircle * randomSpawnRadius;
            // Get a random Debris object by the odds defined
            int randomNumID = GetRandomObjectByOdd();
  
            // Get object from the pool, set world parameters and launch.
            var debris = GetDebrisIDByOdd(randomNumID + 1);
            debris.transform.parent = debrisPoolParent;
            debris.gameObject.SetActive(true);
            debris.transform.position = circleSpawnRandom + spawnPoint.position;
            debris.shootSpeed = (UnityEngine.Random.Range(minVel, maxVel));
            debris.directionAngle = (UnityEngine.Random.Range(minAngle, maxAngle));
            debris.ShootProjectile();
        }

    }



    private int GetRandomObjectByOdd()
    {
        int debrisID = UnityEngine.Random.Range(0, 100);
        int prevI = 0;

        for (int i = 0; i < oddsOfEachPercentAdditive.Length; i++)
        {
            if (debrisID < oddsOfEachPercentAdditive[prevI])
                return i;
            else prevI++;
        }

        return 0;
    }

    private Projectile GetDebrisIDByOdd(int id)
    {
        Projectile tempObj = null;

       switch (id)
         {
             case 1:
                 tempObj = Pool_Debris1.Instance.Get();
                 break;
             case 2:
                 tempObj = Pool_Debris2.Instance.Get();
                 break;
             case 3:
                 tempObj = Pool_Debris3.Instance.Get();
                 break;
             case 4:
                 tempObj = Pool_Debris4.Instance.Get();
                 break;
             case 5:
                 tempObj = Pool_Debris5.Instance.Get();
                 break;
            default:
                 tempObj = Pool_Debris1.Instance.Get();
                 Debug.Log("No debris found, return 1st");
                 break;
                 
         }
        return tempObj;
    }
}
