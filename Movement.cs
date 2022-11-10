using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement: MonoBehaviour
{    
    
    [Header("For Ground and Wall Checks")]
    public Transform groundCheck;
    public bool redBox, greenBox;
    public float redXOffset, redYOffset, redXSize, redYSize, greenXOffset, greenYOffset, greenXSize, greenYSize;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float checkTolerance;   
    Rigidbody2D rb;
    Vector2 playerVelocity;
    #region DIRECTIONS
    float directionX;
    float directionY;
    public float wallDirection;
    #endregion
    [Header("For Animation")]
    //public Animator animator;
    #region ANIMATION BOOLS
    public bool facingFront;
    public bool facingLeft;
    public bool facingRight;
    public bool running;
    #endregion
    [Header("For Movement")]
    #region PUBLIC PHYSICS VARIABLEs    
    public float playerSpeed;
    public float sprintSpeed;
    public float acceleration;
    public float decceleration;
    public float velPower;
    public float frictionAmount;
    #endregion
    [Header("For Jumping")]
    public int jumpCount;
    public float jumpSpeed;
    int jcSave;
    
    [HideInInspector]public bool iTW;
    [HideInInspector]public bool grounded;
    [HideInInspector]public float wallFriction;
    #region CONTROLLERS
    [HideInInspector] public EnergyController _energyController;
    [HideInInspector] public GripController _gripController;
    [HideInInspector] public AuraController _auraController;
    [HideInInspector]public bool isGripping;
    public bool wallSliding;
    [HideInInspector]public bool canRegen = true;
    #endregion

    [Header ("For Wall Climbing and Jumping")]
    public float wcspeed;
    public Vector2 wallJumpThrust;
    bool wallJumping;
    public float verticalOffset;
    public float horizontalOffset;
    [HideInInspector] public bool isw;
    [HideInInspector] public bool canClimb;
    [HideInInspector] public bool isFlipped;
    [HideInInspector] public float startingGrav;
    [HideInInspector] public bool canFlip;
    [HideInInspector] public bool isWallJumping;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public float startdirect;
    private CapsuleCollider2D cc;
    private Vector2 colliderSize;
    [Header("Extras")]
    public float SlopecheckDist = .5f;
    private float slopeDownAngle;
    private Vector2 slopenormalperp;
    private bool isOnSlope;
    private float slopeDownAngleOld;
    [HideInInspector] public bool isSliding;
    [HideInInspector] public float directionsX;
        void Start(){
        rb = GetComponent<Rigidbody2D>(); 
        running = false;
        facingLeft = true;
        facingRight = false;
        jcSave = jumpCount;
        //save = jumpCoolDown;
        _energyController = GetComponent<EnergyController>();
        _gripController = GetComponent<GripController>();
        _auraController = GetComponent<AuraController>();
        isFlipped = false;
        startingGrav = rb.gravityScale;
        canFlip = true;
        cc = GetComponent<CapsuleCollider2D>();
        colliderSize = cc.size;

    }
    void Update(){
        directionsX = directionX;
        if(directionX == -1 && isFlipped)
            flip();
        if(directionX == 1 && !isFlipped)
            flip(); 
            if(isJumping || isWallJumping){
                canFlip = false;
                Vector2 sa = rb.velocity;
                sa.x = Mathf.Abs(sa.x) * startdirect;
            }
        
    
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize), 0f, wallLayer);
        greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize), 0f, wallLayer);

        #region SET DIRECTIONS
        directionX = Input.GetAxisRaw("Horizontal");
        directionY = Input.GetAxisRaw("Vertical");
        playerVelocity.x = directionX;
        playerVelocity.y = directionY;
        
        iTW = isTouchingWall();
        grounded = isGrounded();
        wallSliding = iTW && !grounded;
        Slopecheck();
        if(facingLeft && wallSliding)
            wallDirection = -1;
        if(facingRight && wallSliding)
            wallDirection = 1;
        if(wallSliding){
            canRegen = false;
            }
            if(grounded)
                canRegen = true;
        #endregion
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpCount > 1 && !wallSliding){
            startdirect = Input.GetAxisRaw("Horizontal");
            Jump();
        }
        
        
        if(wallSliding){
            isGripping = true;
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.y), Mathf.Abs(wallFriction));
                amount *= Mathf.Sign(rb.velocity.y);
                rb.AddForce(Vector2.up * -amount, ForceMode2D.Impulse);

            if(Input.GetKey(KeyCode.W)){
                rb.AddForce(new Vector2(0, 100), ForceMode2D.Force);
            }
            if(Input.GetKey(KeyCode.S)){
                rb.AddForce(new Vector2(0, -50), ForceMode2D.Force);
            }
            if(Input.GetKey(KeyCode.LeftShift)){
                //stop downward slide movement
                if(Input.GetKeyDown(KeyCode.Space)){
                    //wallJump can happen
                    rb.AddForce(new Vector2(wallJumpThrust.x * -getWallDirection(), wallJumpThrust.y), ForceMode2D.Impulse);
                    flip();
                }
                if(Input.GetKeyDown(KeyCode.X)){
                    //wallJump can happen
                    rb.AddForce(new Vector2(wallJumpThrust.x * -getWallDirection(), -wallJumpThrust.y * 0.5f), ForceMode2D.Impulse);
                    flip();
                }
            }

        }    
        else
            isGripping = false;     

        if(isGrounded() || isTouchingWall())
            jumpCount = jcSave;
        #region ANIMATOR SET CONDITIONS
        //animator.SetFloat("SpeedX", Mathf.Abs(playerVelocity.x));
        //animator.SetFloat("SpeedY", Mathf.Abs(playerVelocity.y));
        //animator.SetBool("FR", facingRight);
        //animator.SetBool("FL", facingLeft);
        //animator.SetBool("FF", facingFront);
        //animator.SetFloat("Horizontal", playerVelocity.x);
        //animator.SetFloat("Vertical", playerVelocity.y);
        #endregion
        #region FRICTION
        if(isGrounded() && directionX == 0){
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion
    }
    private void Slopecheck(){
        Vector2 checkpos = transform.position - new Vector3(0.0f, colliderSize.y/2);
        SlopecheckVertical(checkpos);
    }
    private void SlopecheckVertical(Vector2 checkpos){
        RaycastHit2D hit = Physics2D.Raycast(checkpos, Vector2.down, SlopecheckDist, groundLayer);
        if(hit){
            slopenormalperp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, slopenormalperp, Color.red);
            if(slopeDownAngle != slopeDownAngleOld){
                isOnSlope = true;
            }
            slopeDownAngleOld = slopeDownAngle;
        }  
    }
    float getWallDirection(){
       return wallDirection;
    }
    public void LetGo(){
        if(facingLeft)
            wallDirection = 1;
                else
            wallDirection = -1;
        rb.AddForce(new Vector2(4 * wallDirection, -3), ForceMode2D.Impulse);
        flip();
        
    }
    public void Slide(){
            rb.AddForce(new Vector2(directionX, 0f), ForceMode2D.Impulse);
    }
    public void Jump(){
            jumpCount -= 1;
            //rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
    void FixedUpdate(){  
        float speed = playerSpeed;
        if(Input.GetKey(KeyCode.LeftShift)){
            speed = sprintSpeed;
        }
        else
            speed = playerSpeed;

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C) && grounded){
            Slide();
        }
        
        #region ANIMATION CONDITIONS
        
            #region MOVEMENT
            float targetSpeed = speed * directionX;
            float speedDiff = targetSpeed - rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
            rb.AddForce(movement * Vector2.right);
            #endregion
        

        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            facingFront = true;
            facingLeft = false;
            facingRight = false;
        }
        if(directionX > 0){
            if(!facingRight && facingLeft)
                flip();
            facingFront = false;
            facingLeft = false;
            facingRight = true;
            
        }
        if(directionX < 0){
            if(!facingLeft && facingRight)
                flip();
            facingFront = false;
            facingRight = false;
            facingLeft = true;
        }
        
        #endregion
        
    }
    void flip(){
        if(canFlip){
        transform.Rotate(0f, 180f, 0f);
        facingLeft = !facingLeft;
        facingRight = !facingRight;
        isFlipped = !isFlipped;
        greenXOffset = -greenXOffset;
        redXOffset = -redXOffset;
        }
    }
    bool isGrounded(){
        bool ig = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkTolerance, groundLayer);
        if(colliders.Length > 0)
            ig = true;
        return ig;
        
    }
    bool isTouchingWall(){   
        return Physics2D.OverlapCircle(wallCheck.position, checkTolerance, wallLayer);
    }
    public void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkTolerance);
        Gizmos.DrawSphere(wallCheck.position, checkTolerance);
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize));
    }
#region OLD COMMENTED OUT CODE
/*
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0){
            playerVelocity.y = jumpSpeed;
            jumpsRemaining--;
        }    
        //moving mechanics
        if ((Input.GetKeyDown(KeyCode.D))){
            playerVelocity.x = playerSpeed;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = right;
        }
        else if ((Input.GetKeyDown(KeyCode.A))){
            playerVelocity.x = -playerSpeed;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = left;
        }
        if ((Input.GetKey(KeyCode.LeftShift))){
            playerVelocity.x = playerSpeed * 2 * x;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftControl)){
                this.gameObject.GetComponent<SpriteRenderer>().sprite = frontCrouch;
                playerVelocity.x = playerSpeed/2 * x;
        }*/

/*
    bool isGrounded()
    {
        bool ig = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkTolerance, groundLayer);
        if(colliders.Length > 0)
            ig = true;
        return ig;
    }
    bool isTouchingWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, checkTolerance, wallLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkTolerance);
        Gizmos.DrawSphere(wallCheck.position, checkTolerance);
    }
    void FacetheFront(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = front;
    }
    void FacetheRight(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sideProfileRight;
    }
    void FacetheLeft(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sideProfileLeft;
    }
    
    void Crouch(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = frontCrouch;
    }
    void CrouchRight(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sideProfileCrouchRight;
    }
    void CrouchLeft(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sideProfileCrouchLeft;
    }
    void Slide(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sliding;
    } */
    #endregion
}