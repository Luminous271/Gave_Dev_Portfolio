using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public float playerSpeed;
    public float playerAccel;
    public int maxJumps;
    public float jumpVelocity;
    public Vector2 wallJumpVelocity;
    public float wallJumpCooldown;
    public float wallSlideSpeed;
    
    public float checkTolerance;

    Rigidbody2D rb;
    Animator anim;

    bool facingRight = true;
    bool wallJumping = false;

    int jumpsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = maxJumps;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float x = Input.GetAxis("Horizontal");
        bool grounded = isGrounded() && rb.velocity.y <= 0.1;
        bool wallSliding = isTouchingWall() && !grounded && !wallJumping;

        Vector2 playerVelocity = rb.velocity;

        if (!wallJumping)
            playerVelocity.x = x * playerSpeed;

        if (x > 0 ^ facingRight && x != 0)
        { //if the facing direction and move direction are different
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
        }

        if (grounded)
            jumpsRemaining = maxJumps;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        { //we pressed jump key
            if (wallSliding)
            { //we are sliding so we should do a wall jump
                playerVelocity.x = -Input.GetAxis("Horizontal") * wallJumpVelocity.x;
                playerVelocity.y = wallJumpVelocity.y;
                wallJumping = true;
                Invoke("stopWallJumping", wallJumpCooldown);
                jumpsRemaining = maxJumps;
            }
            if (jumpsRemaining > 0)
            {
                playerVelocity.y = jumpVelocity;
                jumpsRemaining--;
            }
        }
        else if (wallSliding) //if we are sliding and not jumping
            playerVelocity.y = Mathf.Max(rb.velocity.y, -wallSlideSpeed);

        rb.velocity = playerVelocity;
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkTolerance, groundLayer);
    }

    bool isTouchingWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, checkTolerance, wallLayer);
    }

    void stopWallJumping()
    {
        wallJumping = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkTolerance);
        Gizmos.DrawSphere(wallCheck.position, checkTolerance);
    }
}