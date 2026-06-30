using UnityEngine;


public class CoinMagnetPowerUp : PowerUpBase
{
    
    [SerializeField] public float coinMoveSpeed;
    

    
    protected override void OnActivate()
    {
        player.canMagnet = true;
    }

   
    protected override void OnDeactivate()
    {
        player.canMagnet = false;
    }
}