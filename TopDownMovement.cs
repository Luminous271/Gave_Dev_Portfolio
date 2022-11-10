using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    /* V1 Uneeded Code:
    public Sprite sideProfileRight;
    public Sprite sideProfileLeft;
    public Sprite front;
    public Sprite sideProfileCrouchRight;
    public Sprite sideProfileCrouchLeft;
    public Sprite frontCrouch;
    public Sprite back;*/
    public Animator animator;
    
    Rigidbody2D rb;
    Vector2 playerVelocity;
    float directionX;
    float directionY;
    public float playerSpeed;
    public bool facingfront;
    public bool facingback;
    public bool facingleft;
    public bool facingright;
    public bool running;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        directionY = Input.GetAxisRaw("Vertical");
        playerVelocity.x = directionX;
        playerVelocity.y = directionY;
        animator.SetFloat("Horizontal", playerVelocity.x);
        animator.SetFloat("Vertical", playerVelocity.y);
        animator.SetFloat("Speed", playerVelocity.sqrMagnitude);
        if(playerVelocity.y <= .01){
            animator.SetBool("FF", facingfront);
            animator.SetBool("FB", facingback);
            animator.SetBool("FR", facingright);
            animator.SetBool("FL", facingleft);
        }
        if(playerVelocity.x <= .01){
            animator.SetBool("FF", facingfront);
            animator.SetBool("FB", facingback);
            animator.SetBool("FR", facingright);
            animator.SetBool("FL", facingleft);
        }
    }
    void FixedUpdate()
    {
        //stamina

        //basic movement
        if(directionX > 0){
            rb.MovePosition(rb.position + playerVelocity * playerSpeed * Time.fixedDeltaTime);
            facingback = false;
            facingfront = false;
            facingleft = false;
            facingright = true;
        }
        if(directionX < 0){
            rb.MovePosition(rb.position + playerVelocity * playerSpeed * Time.fixedDeltaTime);
            facingback = false;
            facingfront = false;
            facingleft = true;
            facingright = false;
        }
        if(directionY > 0){
            rb.MovePosition(rb.position + playerVelocity * playerSpeed * Time.fixedDeltaTime);
            facingback = true;
            facingfront = false;
            facingleft = false;
            facingright = false;
        }
        if(directionY < 0){
            rb.MovePosition(rb.position + playerVelocity * playerSpeed * Time.fixedDeltaTime);
            facingback = false;
            facingfront = true;
            facingleft = false;
            facingright = false;
        }
        //sprint
        if((Input.GetKey(KeyCode.LeftShift))){
            rb.MovePosition(rb.position + playerVelocity * playerSpeed * Time.fixedDeltaTime * 2);
            running = true;
        }
    }
    /*void FacetheFront(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = front;
    }
    void FacetheBack(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = back;
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
    }*/
}
