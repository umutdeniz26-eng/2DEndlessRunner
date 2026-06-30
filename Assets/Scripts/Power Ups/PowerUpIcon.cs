using System.Collections;

using UnityEngine;

public class PowerUpIcon : MonoBehaviour
{
    [SerializeField] private float blinkInterval;
    private Player player;
    private Coroutine co;
    private SpriteRenderer sr;

    private float baseScaleX;
    private float originalLocalX;

   
    public static void AddOrRefreshIcon(Player targetPlayer, Sprite iconSprite, float duration)
    {
        PowerUpIcon[] icons = targetPlayer.GetComponentsInChildren<PowerUpIcon>();
        PowerUpIcon emptySlot = null;

        foreach (PowerUpIcon icon in icons)
        {
            
            if (icon.CurrentSprite == iconSprite)
            {
                icon.ShowPowerUp(iconSprite, duration);
                return;
            }
          
            if (icon.CurrentSprite == null && emptySlot == null)
            {
                emptySlot = icon;
            }
        }

       
        if (emptySlot != null)
        {
            emptySlot.ShowPowerUp(iconSprite, duration);
        }
    }

    
    public Sprite CurrentSprite => (co != null) ? sr.sprite : null;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

        baseScaleX = Mathf.Abs(transform.localScale.x);
        originalLocalX = transform.localPosition.x;
    }

    private void Start()
    {
        if (player != null)
        {
            player.OnPlayerTurned += FixIconDir;
            FixIconDir();
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnPlayerTurned -= FixIconDir;
        }
    }

    private void Update()
    {
        if (player.isDead)
        {
            if (co != null)
            {
                StopCoroutine(co);
                co = null;
            }
            sr.sprite = null;
            sr.enabled = false;
        }
    }

    public void ForceClear()
    {
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }

        if (sr != null)
        {
            sr.sprite = null;
            sr.enabled = false;
        }
    }

    public void ShowPowerUp(Sprite powerUpSprite, float duration)
    {
        
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(ShowPowerUpCo(powerUpSprite, duration));
    }

    private IEnumerator ShowPowerUpCo(Sprite powerUpSprite, float duration)
    {
        sr.sprite = powerUpSprite;
        sr.enabled = true;

        float timer = 0f;
        float blinkTimer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (duration - timer <= 3f)
            {
                blinkTimer += Time.deltaTime;

                if (blinkTimer >= blinkInterval)
                {
                    sr.enabled = !sr.enabled;
                    blinkTimer = 0f;
                }
            }
            yield return null;
        }

        sr.sprite = null;
        sr.enabled = false;
        co = null;
    }

    private void FixIconDir()
    {
        float dir = Mathf.Sign(player.transform.localScale.x);
        transform.localScale = new Vector3(baseScaleX * dir, transform.localScale.y, transform.localScale.z);
        transform.localPosition = new Vector3(originalLocalX * dir, transform.localPosition.y, transform.localPosition.z);
    }
}