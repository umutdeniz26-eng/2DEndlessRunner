using System;

using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Movement Details")]
    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float jumpSpeed = 2f;
    [SerializeField] private float doubleJumpSpeed;
    private bool isFacingRight = true;
    private float moveInput;
    private bool canDoubleJump;

    [Header("Collision")]
    [Range(0f, 10f)]
    [SerializeField] private float groundCheckDistance = 3;
    [Range(0f, 10f)]
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.1f, 2f);
    [SerializeField] private Transform wallCheck;
    
    private bool isGrounded;
    private bool isWall;
    public LayerMask isGround;

    [Header("Sliding")]
    [SerializeField] private bool isSliding;
    [SerializeField] private float slidingSpeed;
    [SerializeField] private float slidingDuration = 2f;
    [SerializeField] private float slidingCooldown = 4f;
    [SerializeField] private float slidingTimer;
    [SerializeField] private float lastSlidingTime;
    [SerializeField] private float slideDir;

   
    [Header("Sliding Collider")]
    [SerializeField] private CapsuleCollider2D playerCollider; 
    [SerializeField] private Vector2 slidingColliderSize = new Vector2(0.6f, 1.25f);
    [SerializeField] private Vector2 slidingColliderOffset = new Vector2(0f, -0.8f);
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    [SerializeField] private bool isCeiling;


    [SerializeField] private float ceilingCheckDistance = 1f; 
    [SerializeField] private Vector2 ceilingCheckSize = new Vector2(0.5f, 0.1f);


    private void Start()
    {
        lastSlidingTime = slidingCooldown;

       
        
            originalColliderSize = playerCollider.size;
            originalColliderOffset = playerCollider.offset;
            
        
    }

    private void Update()
    {
        CheckCollision();
        Movement();
        HandleAnimation();
        HandleSliding();
        HandleJump();
        HandleFlip(moveInput);
    }

    private void Movement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        
        

            if (isSliding)
            {
                rb.linearVelocity = new Vector2(slideDir * slidingSpeed, rb.linearVelocity.y);
            }
            else if(!WallCheck())
            {
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            }
        
    }

    private void HandleSliding()
    {
        slidingTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            TrySlide();


        StopSlidingIfNeeded();
    }

    private void StopSlidingIfNeeded()
    {
        isCeiling = CeilingCheck();

        if (slidingTimer < lastSlidingTime - slidingDuration && !isCeiling)
        {
            isSliding = false;
                    
                playerCollider.size = originalColliderSize;
                playerCollider.offset = originalColliderOffset;
            
        }
    }

    private void TrySlide()
    {
        if (slidingTimer < lastSlidingTime - slidingCooldown)
        {
            Slide();
        }
    }

    private void Slide()
    {
        lastSlidingTime = slidingTimer;
        isSliding = true;

        slideDir = isFacingRight ? 1 : -1;
       
            playerCollider.size = slidingColliderSize;
            playerCollider.offset = slidingColliderOffset;
          
        
    }

    private void CheckCollision()
    {
        isWall = WallCheck();
        isGrounded = GroundCheck();
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TryJump();
    }

    private void TryJump()
    {
        if (isGrounded)
        {
            canDoubleJump = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpSpeed);
        }
    }

    public void HandleFlip(float moveInput)
    {
        if (isFacingRight && moveInput < 0)
            Flip();
        else if (isFacingRight == false && moveInput > 0)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x = currentScale.x * -1;
        transform.localScale = currentScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, groundCheckSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + slidingColliderOffset, slidingColliderSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.up * ceilingCheckDistance, ceilingCheckSize);
    }

    private bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, groundCheckSize, 0f, Vector2.down, groundCheckDistance, isGround);
        return hit.collider != null;
    }

    private bool WallCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, isGround);
        return hit.collider != null;
    }

    private bool CeilingCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, ceilingCheckSize, 0, Vector2.up, ceilingCheckDistance,isGround);
        return hit.collider != null;
    }
}