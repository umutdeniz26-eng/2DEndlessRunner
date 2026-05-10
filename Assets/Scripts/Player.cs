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
    [SerializeField] private bool isFacingRight = true;
    private float moveInput;

    [Header("Collision")]
    [Range(0f,10f)]
    [SerializeField] private float rayDistance = 3;
    public LayerMask isGround;
    [SerializeField] private bool grounded;

    


    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        HandleAnimation();

        HandleJump();
        HandleFlip(moveInput);
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(moveInput));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", grounded);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&grounded)
            Jump();
    }

    

    public void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        grounded = GroundCheck();
    }


    public void HandleFlip(float moveInput)
    {
        if (isFacingRight && moveInput < 0)
            Flip();

        else if(isFacingRight==false && moveInput > 0)
            Flip();
    }


    public void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 currentScale=transform.localScale;
        currentScale.x = currentScale.x * -1;
        transform.localScale=currentScale;
    }


    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;


        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistance);
    }

    private bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, isGround);
        return hit;
    }
}
