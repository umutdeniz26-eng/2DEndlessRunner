using System.Collections;

using UnityEngine;

public class CoinMagnetPowerUp : MonoBehaviour
{
    [SerializeField] private float amp=0.0002f;
    [SerializeField] private float fr=4;
    Player player;
    private Coroutine co;
    private float startPosY;
    private bool canMagnet;
    private float duration = 5;


   

    private void Start()
    {
        startPosY=transform.position.y;
    }
    private void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + Mathf.Sin(Time.time * fr) * amp);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player>();
        if (player != null && co == null)
            co = StartCoroutine(PullCoin());

    }
    private IEnumerator PullCoin()
    {
        player.canMagnet = true;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;


        yield return new WaitForSeconds(duration);

        player.canMagnet = false;

        co = null;

        Destroy(gameObject);

    }
}
