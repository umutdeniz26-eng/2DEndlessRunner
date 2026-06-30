using UnityEngine;

public class ScoreMultiplierPowerUp : PowerUpBase
{

    [SerializeField] private float scoreMultiplierEffect;
  

    private GameManager gameManager;
    private float originalMult;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    protected override void OnActivate()
    {
        originalMult = gameManager.scoreMultiplier;
        gameManager.scoreMultiplier *= scoreMultiplierEffect;
    }

    protected override void OnDeactivate()
    {
       gameManager.scoreMultiplier = originalMult;
    }
}
