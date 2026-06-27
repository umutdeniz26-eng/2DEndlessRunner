using System.Collections;

using UnityEngine;

public class FastRunPowerUp : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] private float ghostSpawnRate;

    [SerializeField] private Color powerUpColor;

    [SerializeField] private float duration;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float amp;
    [SerializeField] private float fr;
    private float originalPosY;
    Player player;
    private Coroutine co;


    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void Start()
    {
        originalPosY= transform.position.y;
    }
    private void Update()
    {
        transform.position = new Vector2(transform.position.x,  originalPosY + Mathf.Sin(Time.time * fr) * amp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
            if (co == null)
                co = StartCoroutine(SpeedUpCo());
                
    }


    private IEnumerator SpeedUpCo()
    {
        player.isInvincible = true;

        float originalMoveSpeed = player.currentMoveSpeed;
        player.currentMoveSpeed *= speedMultiplier;

        SpriteRenderer playerSr=player.GetComponentInChildren<SpriteRenderer>();
        Color originalColor = playerSr.color;
        playerSr.color = powerUpColor;

        PowerUpIcon[] icons = player.GetComponentsInChildren<PowerUpIcon>();
        foreach (PowerUpIcon icon in icons)
        {
            if (icon.ShowPowerUp(GetComponent<SpriteRenderer>().sprite, duration))
            {

                break;
            }

        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;


        float timer = 0f;
        float ghostTimer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            ghostTimer-= Time.deltaTime;

            if (ghostTimer <= 0f)
            {
                SpawnGhost();
                ghostTimer = ghostSpawnRate;
            }


            yield return null;
        }



        player.currentMoveSpeed = originalMoveSpeed;

        playerSr.color = originalColor;

        player.isInvincible = false;
        Destroy(gameObject);
    }



    private void SpawnGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, player.transform.position, player.transform.rotation);

        SpriteRenderer ghostSr=ghost.GetComponent<SpriteRenderer>();
        SpriteRenderer playerSr=player.GetComponentInChildren<SpriteRenderer>();

        ghostSr.sprite = playerSr.sprite;
        ghostSr.flipX = playerSr.flipX;
        ghostSr.flipY= playerSr.flipY;
        ghostSr.color = playerSr.color;


        ghostSr.sortingLayerName=playerSr.sortingLayerName;
        ghostSr.sortingOrder = playerSr.sortingOrder - 1;

    }
}
