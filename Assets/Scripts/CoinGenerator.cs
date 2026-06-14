using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Coin;
    [SerializeField] private int minCoinAmount;
    [SerializeField] private int maxCoinAmount;
    [SerializeField] private  float coinGenerateOffset;
    private int amountOfCoins;


    private void Awake()
    {
        amountOfCoins=Random.Range(minCoinAmount,maxCoinAmount);


        for (int i = 0; i < amountOfCoins; i++)
        {
         Vector3 offset=new Vector2(i*coinGenerateOffset,0);
        Instantiate(Coin,transform.position+offset,Quaternion.identity,transform);
        }
    }






}
