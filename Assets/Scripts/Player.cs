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
    private bool canDoubleJump;
    private float moveInput;




    [Header("Collision")]

    [Range(0f, 10f)]
    [SerializeField] private float groundCheckDistance = 3;
    [Range(0f, 10f)]
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.1f, 2f);
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask isGround;
    private bool isWallLedge;
    private bool isGrounded;
    private bool isLedge;
    private bool isWall;



    [Header("Sliding")]

    [SerializeField] private float slidingSpeed;
    [SerializeField] private float slidingDuration = 2f;
    [SerializeField] private float slidingCooldown = 4f;
    [SerializeField] private float slideWallCheckDistance = 1.5f;
    [SerializeField] private Vector2 slideWallCheckOffset = new Vector2(0f, -0.8f);
    private bool isSliding;
    private float slidingTimer;
    private float lastSlidingTime;
    private float slideDir;
    private bool isSlideWallCheck;




    [Header("Sliding Collider")]

    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private Vector2 slidingColliderSize = new Vector2(0.6f, 1.25f);
    [SerializeField] private Vector2 slidingColliderOffset = new Vector2(0f, -0.8f);
    [SerializeField] private Vector2 ceilingCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private float ceilingCheckDistance = 1f;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private bool isCeiling;


    [Header("Ledge")]

    [SerializeField] private Vector2 hangingOffset;
    [SerializeField] private Vector2 standOffset;
    [SerializeField] private Vector2 climbCeilingCheckOffset = new Vector2(0.5f, 1f);
    [SerializeField] private Vector2 ledgeJump;
    [SerializeField] private float climbCeilingCheckDistance = 2f;
    [SerializeField] private float ledgeCheckDistance = 0.5f;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform ledgeWallCheck;
    private bool isTouchingLedgeWall;
    private bool isTouchingLedge;
    private bool isHanging;
    private bool isClimbing;
    private bool isClimbCeiling;
    private Vector2 climbOverPosition;
    private Vector2 climbBeginPosition;



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
        HandleLedge();
    }



    private void HandleLedge()
    {
        if (isTouchingLedgeWall && !isTouchingLedge && !isHanging && !isGrounded)
        {

            SnapToCorner();
        }

        HandleHanging();
    }



    private void HandleHanging()
    {
        if (isHanging)
        {
            canDoubleJump = true;

            isClimbCeiling = ClimbCeilingCheck();
            float dir = isFacingRight ? 1f : -1f;

            if (Input.GetKeyDown(KeyCode.Space) && Input.GetAxisRaw("Horizontal") * dir < 0)
            {
                isHanging = false;
                rb.linearVelocity = ledgeJump;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !isClimbCeiling)
            {
                isHanging = false;
                isClimbing = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && isClimbCeiling)
            {

            }
        }
    }

    public void FinishClimb()
    {
        isClimbing = false;
        float dir = isFacingRight ? 1f : -1f;
        Vector2 correctedOffset = new Vector2(standOffset.x * dir, standOffset.y);
        transform.position = (Vector2)transform.position + correctedOffset;
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.Play("idle/move");
        anim.Update(0f);
    }

    

    private bool SnapToCorner()
    {

        isHanging = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        float dir = isFacingRight ? 1f : -1f;
        RaycastHit2D wallHit = Physics2D.Raycast(ledgeWallCheck.position, Vector2.right * dir, ledgeCheckDistance, isGround);

        if (wallHit.collider != null)
        {
            Vector2 downRayStart = new Vector2(wallHit.point.x + (dir * 0.05f), ledgeCheck.position.y + 0.1f);
            RaycastHit2D groundHit = Physics2D.Raycast(downRayStart, Vector2.down, 1.5f, isGround);

            if (groundHit.collider != null)
            {
                Vector2 exactCorner = new Vector2(wallHit.point.x, groundHit.point.y);
                climbBeginPosition = new Vector2(exactCorner.x + (hangingOffset.x * dir), exactCorner.y + hangingOffset.y);
                transform.position = climbBeginPosition;
                return false;
                
            }
        }

        climbBeginPosition = (Vector2)ledgeWallCheck.position + hangingOffset;
        transform.position = climbBeginPosition;
        return true;
       
    }
    
    private void Movement()
    {
        if (isHanging|| isClimbing) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        if (isSliding)
        {
            rb.linearVelocity = new Vector2(slideDir * slidingSpeed, rb.linearVelocity.y);
        }
        else if (!WallCheck())
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
        isSlideWallCheck = SlideWallCheck();

        if ((slidingTimer < lastSlidingTime - slidingDuration && !isCeiling) || isSlideWallCheck)
        {
            isSliding = false;
            playerCollider.size = originalColliderSize;
            playerCollider.offset = originalColliderOffset;
        }
    }

    private void TrySlide()
    {
        if (slidingTimer < lastSlidingTime - slidingCooldown && isHanging == false && rb.linearVelocity.y == 0)
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
        float dir = isFacingRight ? 1f : -1f;

        isWall = WallCheck();
        isGrounded = GroundCheck();

       
        
            isTouchingLedgeWall=LedgeWallCheck(dir);
            isTouchingLedge = LedgeCheck(dir);
        
    }

    
    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("isHanging", isHanging);
        anim.SetBool("isClimbing", isClimbing);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isHanging && !isClimbing)
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
        if (isHanging || isClimbing || isSliding) return;

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


        float dir = isFacingRight ? 1f : -1f;


        //GroundCheck Gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, groundCheckSize);

        //SlidingCollider Gizmo
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + slidingColliderOffset, slidingColliderSize);

        //CeilingChec kGizmo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.up * ceilingCheckDistance, ceilingCheckSize);

        //SlideWallCheck Gizmo
        Gizmos.color = Color.yellow;
        Vector2 slideStartPos = (Vector2)transform.position + slideWallCheckOffset;
        Gizmos.DrawLine(slideStartPos, slideStartPos + Vector2.right * dir * slideWallCheckDistance);

        //ClimbCeiling Gizmo
        Gizmos.color = Color.magenta;
        Vector2 climbCeilingStart = (Vector2)transform.position + new Vector2(climbCeilingCheckOffset.x * dir, climbCeilingCheckOffset.y);
        Gizmos.DrawLine(climbCeilingStart, climbCeilingStart + Vector2.up * climbCeilingCheckDistance);

       
        //LedgeCheck and LedgeWallCheck Gizmo
         Gizmos.color = Color.cyan;
         Gizmos.DrawLine(ledgeWallCheck.position, ledgeWallCheck.position + Vector3.right * dir * ledgeCheckDistance);
         Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + Vector3.right * dir * ledgeCheckDistance);
        
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
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, ceilingCheckSize, 0, Vector2.up, ceilingCheckDistance, isGround);
        return hit.collider != null;
    }

    private bool SlideWallCheck()
    {
        float dir = isFacingRight ? 1f : -1f;
        Vector2 startPos = (Vector2)transform.position + slideWallCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.right * dir, slideWallCheckDistance, isGround);
        return hit.collider != null;
    }

    private bool ClimbCeilingCheck()
    {
        float dir = isFacingRight ? 1f : -1f;
        Vector2 startPos = (Vector2)transform.position + new Vector2(climbCeilingCheckOffset.x * dir, climbCeilingCheckOffset.y);
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.up, climbCeilingCheckDistance, isGround);
        return hit.collider != null;
    }

    private bool LedgeCheck(float dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeCheck.position, Vector2.right * dir, ledgeCheckDistance, isGround);
        return hit.collider != null;
    }

    private bool LedgeWallCheck(float dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeWallCheck.position, Vector2.right * dir, ledgeCheckDistance, isGround);
        return hit.collider != null;
    }

}