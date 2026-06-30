using UnityEngine;

public class CoinMulitplierPowerUp : PowerUpBase
{
    [SerializeField] private int coinMultiplierEffect;
  

    GameManager gameManager;
    private int originalMult;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    protected override void OnActivate()
    {
        originalMult = gameManager.coinMultiplier;
        gameManager.coinMultiplier=coinMultiplierEffect;
    }

    protected override void OnDeactivate()
    {
        gameManager.coinMultiplier = originalMult;
    }
    

}
