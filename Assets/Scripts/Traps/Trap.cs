using UnityEngine;

public class Trap : MonoBehaviour
{

    protected Animator anim;
    protected Collider2D trapCollider;

    protected bool isActivated;
    protected bool canActivated = true;


    protected virtual void Awake()
    {
     anim=GetComponentInChildren<Animator>();
        trapCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        
    }

    
    protected virtual void Update()
    {
        AnimationController();
    }

    protected virtual void AnimationController()
    {
        anim.SetBool("isActivated", isActivated);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = collision.GetComponent<Player>();
        if (player != null && canActivated)
        {
            isActivated = true;
            

            player.TakeDamage();
            canActivated = false;


            trapCollider.isTrigger = false;

            gameObject.layer = LayerMask.NameToLayer("Ground");

        }

        



    }
   

    
}
