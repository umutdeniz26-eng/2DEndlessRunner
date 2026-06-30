using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public abstract class PowerUpBase : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected float duration = 5f;
    [SerializeField] private float amp = 0.1f;
    [SerializeField] private float fr = 4f;

    protected Player player;
    private float startPosY;

   
    protected float timer = 0f;

    
    private static Dictionary<System.Type, PowerUpBase> activePowerUps = new Dictionary<System.Type, PowerUpBase>();

    private void Start()
    {
        startPosY = transform.position.y;
        OnInit(); 
    }

    
    protected virtual void OnInit() { }

    private void Update()
    {
       
        transform.position = new Vector2(transform.position.x, startPosY + Mathf.Sin(Time.time * fr) * amp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player hitPlayer = collision.GetComponent<Player>();
        if (hitPlayer != null)
        {
            player = hitPlayer;

            
            System.Type myType = this.GetType();

            if (activePowerUps.ContainsKey(myType) && activePowerUps[myType] == null)
            {
                activePowerUps.Remove(myType); 
            }

            if (activePowerUps.ContainsKey(myType) && activePowerUps[myType] != this)
            {
                
                activePowerUps[myType].timer = 0f;
                PowerUpIcon.AddOrRefreshIcon(player, GetComponent<SpriteRenderer>().sprite, duration);
                Destroy(gameObject);
            }
            else if (!activePowerUps.ContainsKey(myType))
            {
                
                activePowerUps[myType] = this;
                StartCoroutine(PowerUpRoutine());
            }
        }
    }

    private IEnumerator PowerUpRoutine()
    {
        PowerUpIcon.AddOrRefreshIcon(player, GetComponent<SpriteRenderer>().sprite, duration);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        
        OnActivate();

        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            
            OnPowerUpUpdate();

            yield return null;
        }

       
        if (player != null)
        {
            
            OnDeactivate();
        }

        
        activePowerUps.Remove(this.GetType());
        Destroy(gameObject);
    }

    public static void ClearAllPowerUps()
    {
     
        foreach (var kvp in activePowerUps)
        {
            PowerUpBase powerUp = kvp.Value;
            if (powerUp != null)
            {
                powerUp.StopAllCoroutines(); 

                if (powerUp.player != null)
                {
                    powerUp.OnDeactivate(); 
                }

                Destroy(powerUp.gameObject); 
            }
        }

        
        activePowerUps.Clear();
    }
    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
    protected virtual void OnPowerUpUpdate() { } 
}