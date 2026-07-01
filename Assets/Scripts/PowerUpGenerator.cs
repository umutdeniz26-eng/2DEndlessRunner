using UnityEngine;

public class PowerUpGenerator : MonoBehaviour
{

    [SerializeField] PowerUpBase[] powerUps;
    [SerializeField] private float chanceToSpawn;

    void Start()
    {
        if (chanceToSpawn > Random.Range(0, 100))
        {
            PowerUpBase powerUpToCreate = powerUps[Random.Range(0, powerUps.Length)];
            PowerUpBase spawnedPowerUp = Instantiate(powerUpToCreate, transform.position, Quaternion.identity);

            
            spawnedPowerUp.transform.SetParent(transform);

            Vector3 parentGlobalScale = transform.lossyScale;
            spawnedPowerUp.transform.localScale = new Vector3(1f / parentGlobalScale.x, 1f / parentGlobalScale.y, 1f / parentGlobalScale.z);
        }
    }

   
    void Update()
    {
        
    }
}
