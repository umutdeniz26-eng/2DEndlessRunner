using UnityEngine;

public class HealthPowerUp : PowerUpBase
{

    [SerializeField] private int healthToIncrease;
    

    
    private int originalHealth;



    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }
    protected override void OnActivate()
    {
        originalHealth=player.playerHealth;
        player.playerHealth += healthToIncrease;
    }

    protected override void OnDeactivate()
    {
        if(player.playerHealth==originalHealth+healthToIncrease)
            player.playerHealth = originalHealth;
    }
}
