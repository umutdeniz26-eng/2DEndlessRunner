using System.Collections;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Components")]

   [SerializeField] private Rigidbody2D rb;
   [SerializeField] private Animator anim;
    private SpriteRenderer sr;
    private Color invincibilityColor = new Color32(255, 255, 255, 120);
    private Color originalColor;



    [Header("Movement Details")]

    [SerializeField] public float currentMoveSpeed;
    [SerializeField] private float defaultMoveSpeed = 9f;   
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float doubleJumpSpeed=10f;
    [SerializeField] private Vector3 knockbackSpeed = new Vector3(-8, 3, 0);
    [SerializeField] private float canRollThreshold = -20f;
    [Space]
    [SerializeField] private float speedMultiplier = 1.01f;
    [SerializeField] private float speedIncreaseCooldown = 4;
    [SerializeField] private float maxMoveSpeed = 20;
    [SerializeField] private float maxRunAnimSpeed = 1.8f;
    public Action OnPlayerTurned;
    private float speedTimer = 0;
    private float runAnimSpeed = 1;
    private bool isKnockback;
    private bool canRoll;
    private bool isFacingRight = true;
    private bool canDoubleJump = true;
    private float moveInput;


   

    [Header("Collision")]

    [Range(0f, 10f)]
    [SerializeField] private float groundCheckDistance = 1.29f;
    [Range(0f, 10f)]
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.1f, 2f);
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private LayerMask isTrap;
    private bool isTouchingTrap;
    private bool isWallLedge;
    private bool isGrounded;
    private bool isLedge;
    private bool isWall;
    public bool canMagnet;



    [Header("Sliding")]

    [SerializeField] private float slideSpeedMultiplier = 1.2f;  
    [SerializeField] private float slidingDuration = 0.8f;
    [SerializeField] private float slidingCooldown = 3f;
    [SerializeField] private float slideWallCheckDistance = 1.2f;
    [SerializeField] private Vector2 slideWallCheckOffset = new Vector2(0f, -1.21f);
    [SerializeField] private float slidingSpeed = 12;
    private bool isSliding;
    private float slidingTimer;
    private float lastSlidingTime;
    private float slideDir;
    private bool isSlideWallCheck;




    [Header("Sliding Collider")]

    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private Vector2 slidingColliderSize = new Vector2(0.6f, 1.25f);
    [SerializeField] private Vector2 slidingColliderOffset = new Vector2(0f, -0.71f);
    [SerializeField] private Vector2 ceilingCheckSize = new Vector2(0.5f, 1.3f);
    [SerializeField] private float ceilingCheckDistance = 0.45f;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private bool isCeiling;


    [Header("Ledge")]

    [SerializeField] private Vector2 hangingOffset = new Vector2(-0.46f, 1.25f);
    [SerializeField] private Vector2 standOffset = new Vector2(0.872f, 2.519f);
    [SerializeField] private Vector2 climbCeilingCheckOffset = new Vector2(0.5f, 1.3f);
    [SerializeField] private Vector2 ledgeJump = new Vector2(3, 12);
    [SerializeField] private float climbCeilingCheckDistance = 2.2f;
    [SerializeField] private float ledgeCheckDistance = 0.6f;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform ledgeWallCheck;
    private bool isTouchingLedgeWall;
    private bool isTouchingLedge;
    private bool isHanging;
    private bool isClimbing;
    private bool isClimbCeiling;
    private Vector2 climbOverPosition;
    private Vector2 climbBeginPosition;


    [Header("Invincibility")]
    [SerializeField] private float invincibilityDuration=3f;
    [SerializeField] private float blinkDuration = 0.25f;
    [SerializeField] private float damageMultiplier = 1.5f;
    public bool isInvincible;
    public bool isDead;
    private bool canBeKnocked = true;



    private void Start()
    {
        
        sr=GetComponentInChildren<SpriteRenderer>();

        lastSlidingTime = slidingCooldown;
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
        currentMoveSpeed = defaultMoveSpeed;
        originalColor = sr.color;
    }

    private void Update()
    {
        CheckCollision();
        HandleAnimation();
        HandleDeath();

        if (isDead)
            return;

        Movement();
        HandleSliding();
        HandleJump();
        HandleFlip(moveInput);
        HandleLedge();
        HandleSpeedControl();
        HandleRoll();
        HandleKnockback();
        
    }

    private void HandleDeath()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isDead)
            Die();

    }

    public void TakeDamage()
    {
        if(!isInvincible)
           Damage();
    }

    private void Damage()
    {
        if (currentMoveSpeed >= defaultMoveSpeed * damageMultiplier)
        {
            Knockback();
        }

        else Die();
    }

    private void Die()
    {
        float dir = isFacingRight ? 1 : -1;
        isDead = true;

        if (isHanging || isClimbing)
        {
           
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = new Vector3(knockbackSpeed.x * dir, knockbackSpeed.y, 0f);
        }


        rb.linearVelocity = new Vector3(knockbackSpeed.x*dir,knockbackSpeed.y,0); 
        
    }

    public void EndKnockback()
    {
        isKnockback = false;
        StartCoroutine(Invincibility());
    }

    private void HandleKnockback()
    {


        if (Input.GetKeyDown(KeyCode.K) && canBeKnocked)
            Knockback();
    }

    private void Knockback()
    {

        isKnockback = true;
        rb.linearVelocity = knockbackSpeed;
        ResetSpeed();
        
    }
    
    private IEnumerator Invincibility()
    {
        float time = 0f;

        canBeKnocked = false;

        while (time < invincibilityDuration)
        {
        sr.color = invincibilityColor;
        yield return new WaitForSeconds(blinkDuration);
        sr.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);

            time += blinkDuration*2;
        }

        canBeKnocked = true;
        

    }

    private void HandleSpeedControl()
    {
        if (isWall && !isHanging && !isClimbing && !isInvincible)
        {
            ResetSpeed();
        }


        if (currentMoveSpeed == maxMoveSpeed)
            return;

        if (rb.linearVelocity.x > 0.1f) 
        speedTimer += Time.deltaTime;


       

        if (speedTimer > speedIncreaseCooldown)
        {
            speedTimer = 0;
             
        currentMoveSpeed = currentMoveSpeed * speedMultiplier;
            runAnimSpeed*= speedMultiplier;

            if(currentMoveSpeed>maxMoveSpeed)
                 currentMoveSpeed = maxMoveSpeed;
            if(runAnimSpeed>maxRunAnimSpeed)
                runAnimSpeed = maxRunAnimSpeed;
        }



    }

    public void ResetSpeed()
    {
        currentMoveSpeed = defaultMoveSpeed;
        runAnimSpeed = 1;
        speedTimer = 0;
    }

    private void HandleLedge()
    {
        if (isTouchingLedgeWall && !isTouchingLedge && !isHanging && !isGrounded )
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
        if (isHanging|| isClimbing || isKnockback || isDead ) return;

        

        moveInput = Input.GetAxisRaw("Horizontal");

        if (isSliding)
        {
            rb.linearVelocity = new Vector2(slideDir * slidingSpeed, rb.linearVelocity.y);
        }
        else if (!WallCheck() && !isTouchingTrap)
        {
            rb.linearVelocity = new Vector2(moveInput * currentMoveSpeed, rb.linearVelocity.y);
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
        slidingSpeed = currentMoveSpeed * slideSpeedMultiplier;
    }

    private void CheckCollision()
    {
        float dir = isFacingRight ? 1f : -1f;

        isWall = WallCheck();
        isGrounded = GroundCheck();
        isTouchingTrap = TrapCheck();

       
        
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
        anim.SetBool("canRoll", canRoll);
        anim.SetBool("isKnockback", isKnockback);
        anim.SetBool("isDead", isDead);
        anim.SetFloat("runAnimSpeed", runAnimSpeed);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isHanging && !isClimbing && !isDead &&!isSliding)
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
        OnPlayerTurned?.Invoke();
    }


    public void EndRoll()
    {
        canRoll = false;
    }

    private void HandleRoll()
    {
        if (rb.linearVelocity.y < canRollThreshold)
            canRoll = true;
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

        //CeilingCheck Gizmo
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

    private bool TrapCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, isTrap);
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