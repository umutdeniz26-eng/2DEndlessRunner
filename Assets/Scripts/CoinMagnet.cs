using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    Player player;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Coin coin=collision.GetComponent<Coin>();
        if (coin!=null && player.canMagnet) 
            coin.TargetPlayer(transform.parent.position);


    }
}
