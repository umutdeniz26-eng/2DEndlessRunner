using UnityEngine;

public class Trap : MonoBehaviour
{

    private Animator anim;
    private Collider2D trapCollider;

    private bool isActivated;
    private bool canActivated = true;


    private void Awake()
    {
     anim=GetComponentInChildren<Animator>();
        trapCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = collision.GetComponent<Player>();
        if (player != null && canActivated)
        {
            isActivated = true;
            anim.SetBool("isActivated", isActivated);

            player.TakeDamage();
            canActivated = false;


            trapCollider.isTrigger = false;

            gameObject.layer = LayerMask.NameToLayer("Ground");

        }

        



    }
   

    
}
