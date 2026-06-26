using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float moveSpeed=12;


    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

   
    void Update()
    {
        rb.linearVelocity=new Vector2(moveSpeed* (-1),rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null) 
            player.TakeDamage();
    }
}
