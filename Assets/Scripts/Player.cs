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
    [Range(0f,10f)]
    [SerializeField] private float wallCheckDistance = 1;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.1f, 2f);
    [SerializeField] private Transform wallCheck;
    private bool isGrounded;
    private bool isWall;
    public LayerMask isGround;

    


    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        CheckCollision();

        Movement();

        HandleAnimation();

        HandleJump();
        HandleFlip(moveInput);

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


    

    private void Movement()
    {

        if (!WallCheck())
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
    }

    public void HandleFlip(float moveInput)
    {
        if (isFacingRight && moveInput < 0)
            Flip();

        else if (isFacingRight == false && moveInput > 0)
            Flip();
    }


    public void Flip()
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
       
    }

    private bool GroundCheck()
    {
        
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, groundCheckSize, 0f, Vector2.down, groundCheckDistance, isGround);
        return hit.collider != null;
    }

    private bool WallCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(wallCheck.position,wallCheckSize,0,Vector2.zero,0,isGround);
        return hit.collider != null;
    }

}