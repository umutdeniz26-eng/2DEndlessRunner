using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject coinLevel1;
    [SerializeField] private GameObject coinLevel2;
    [SerializeField] private GameObject coinLevel3;
    [SerializeField] private GameObject diamond;
     private Transform firstCoinSpawnPos;


    [Header("CoinChances")]
    [SerializeField] private float noCoinChance;
    [SerializeField] private float coinLevel1Chance;
    [SerializeField] private float coinLevel2Chance;
    [SerializeField] private float coinLevel3Chance;
    [SerializeField] private float diamondChance;


    [SerializeField] private float coinSpawnDelay;
    
    [SerializeField] private int minCoinAmount;
    [SerializeField] private int maxCoinAmount;


    private void Awake()
    {
         firstCoinSpawnPos=this.transform;
    }
    void Start()
    {
       int coinCount = Random.Range(minCoinAmount, maxCoinAmount);

        for (int i = 0; i < coinCount; i++)
        {
            
            float coinLevel = Random.Range(0, 100);
            if (coinLevel>noCoinChance && coinLevel < coinLevel1Chance)
            {
                CreateLevel1Coin(i);

            }

            if (coinLevel > coinLevel1Chance && coinLevel < coinLevel2Chance)
            {
                CreateLevel2Coin(i);
            }

            if (coinLevel > coinLevel2Chance && coinLevel < coinLevel3Chance)
            {
                CreateLevel3Coin(i);
            }

            if (coinLevel> diamondChance)
            {
                CreateDiamond(i);
            }

        }
    }
    private void CreateLevel1Coin(float i)
    {
        Vector3 spawnPos = firstCoinSpawnPos.position + new Vector3(coinSpawnDelay * i, 0, 0);

        
        GameObject spawnedCoin = Instantiate(coinLevel1, spawnPos, Quaternion.identity);

      
        spawnedCoin.transform.SetParent(transform);

       
        Vector3 parentGlobalScale = transform.lossyScale;
        spawnedCoin.transform.localScale = new Vector3(1f / parentGlobalScale.x, 1f / parentGlobalScale.y, 1f / parentGlobalScale.z);




    }

    
    private void CreateLevel2Coin(float i)
    {
        Vector3 spawnPos = firstCoinSpawnPos.position + new Vector3(coinSpawnDelay * i, 0, 0);

       
        GameObject spawnedCoin = Instantiate(coinLevel2, spawnPos, Quaternion.identity);

        spawnedCoin.transform.SetParent(transform);

     
        Vector3 parentGlobalScale = transform.lossyScale;
        spawnedCoin.transform.localScale = new Vector3(1f / parentGlobalScale.x, 1f / parentGlobalScale.y, 1f / parentGlobalScale.z);





    }

    private void CreateLevel3Coin(float i)
    {
        Vector3 spawnPos = firstCoinSpawnPos.position + new Vector3(coinSpawnDelay * i, 0, 0);

        GameObject spawnedCoin = Instantiate(coinLevel3, spawnPos, Quaternion.identity);

  
        spawnedCoin.transform.SetParent(transform);

        
        Vector3 parentGlobalScale = transform.lossyScale;
        spawnedCoin.transform.localScale = new Vector3(1f / parentGlobalScale.x, 1f / parentGlobalScale.y, 1f / parentGlobalScale.z);





    }

    private void CreateDiamond(float i)
    {
        Vector3 spawnPos = firstCoinSpawnPos.position + new Vector3(coinSpawnDelay * i, 0, 0);


        GameObject spawnedCoin = Instantiate(diamond, spawnPos, Quaternion.identity);

     
        spawnedCoin.transform.SetParent(transform);

      
        Vector3 parentGlobalScale = transform.lossyScale;
        spawnedCoin.transform.localScale = new Vector3(1f / parentGlobalScale.x, 1f / parentGlobalScale.y, 1f / parentGlobalScale.z);





    }
    void Update()
    {
        
    }
}
