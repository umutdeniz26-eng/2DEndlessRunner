using UnityEngine;

public class Coin : MonoBehaviour,ICollectible
{

    [SerializeField] private int coinValue;
    Player player;
    CoinMagnetPowerUp magnet;
    GameManager gameManager;
    private float coinMoveSpeed;
    private Rigidbody2D rb;
    private Vector2 targetPos;
    private bool shouldMove;
    public bool isMagnet;
    
    void Start()
    {
        if(magnet!=null)
          coinMoveSpeed = magnet.coinMoveSpeed;
    }

    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        player= FindAnyObjectByType<Player>();
        gameManager = FindAnyObjectByType<GameManager>();
        magnet = FindAnyObjectByType<CoinMagnetPowerUp>();
    }

    void Update()
    {
        isMagnet = player.canMagnet;
        if(shouldMove && isMagnet)
          MoveToPlayer();

        else rb.linearVelocity= Vector2.zero;
    }

    private void MoveToPlayer()
    {
        
        rb.linearVelocity=new Vector2(targetPos.x,targetPos.y)*coinMoveSpeed;
    }

    public void TargetPlayer(Vector2 Position)
    {
        shouldMove = true;
         targetPos = Position - (Vector2)transform.position ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            int coinToAdd = gameManager.coinMultiplier*coinValue;
            GameManager.gameManager.coins += coinToAdd;
        Destroy(gameObject);
        

        }
    }




}
