using UnityEngine;


public class FastRunPowerUp : PowerUpBase
{
    [Header("Fast Run Settings")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostSpawnRate;
    [SerializeField] private Color powerUpColor;
    [SerializeField] private float speedMultiplier;
    
   
    private float originalMoveSpeed;
    private Color originalColor;
    private SpriteRenderer playerSr;
    private float ghostTimer = 0f;

    
    protected override void OnActivate()
    {
        player.isInvincible = true;

        originalMoveSpeed = player.currentMoveSpeed;
        player.currentMoveSpeed *= speedMultiplier;

        playerSr = player.GetComponentInChildren<SpriteRenderer>();
        if (playerSr != null)
        {
            originalColor = playerSr.color;
            playerSr.color = powerUpColor;
        }
    }

    
    protected override void OnPowerUpUpdate()
    {
        ghostTimer -= Time.deltaTime;
        if (ghostTimer <= 0f)
        {
            SpawnGhost();
            ghostTimer = ghostSpawnRate; 
        }
    }

   
    protected override void OnDeactivate()
    {
        player.currentMoveSpeed = originalMoveSpeed;
        if (playerSr != null)
        {
            playerSr.color = originalColor;
        }
        player.isInvincible = false;
    }

    
    private void SpawnGhost()
    {
        if (playerSr == null) return;

        GameObject ghost = Instantiate(ghostPrefab, player.transform.position, player.transform.rotation);
        SpriteRenderer ghostSr = ghost.GetComponent<SpriteRenderer>();

        ghostSr.sprite = playerSr.sprite;
        ghostSr.flipX = playerSr.flipX;
        ghostSr.flipY = playerSr.flipY;
        ghostSr.color = playerSr.color;

        ghostSr.sortingLayerName = playerSr.sortingLayerName;
        ghostSr.sortingOrder = playerSr.sortingOrder - 1;
    }
}