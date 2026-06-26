using UnityEngine;

public class Coin : MonoBehaviour,ICollectible
{

    [SerializeField] private float moveSpeed;
    Player player;
    private Rigidbody2D rb;
    private Vector2 targetPos;
    private bool shouldMove;
    public bool isMagnet;
    
    void Start()
    {

    }

    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        player= FindAnyObjectByType<Player>();
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
        
        rb.linearVelocity=new Vector2(targetPos.x,targetPos.y)*moveSpeed;
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
        GameManager.instance.coins++;
        Destroy(gameObject);
        

        }
    }




}
