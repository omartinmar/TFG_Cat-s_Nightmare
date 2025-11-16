using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    [SerializeField] private float speed = 7;
    [SerializeField] private float jumpForce = 11;
    private BoxCollider2D boxCollider;



    private float horizontalInput;
    private float verticalInput;
    private Animator animator;


    [SerializeField] private float wallSlidingSpeed = 0.7f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;




    void Awake()  
    {
        //Grab references for rigid body and animator from object
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    //Get inputs
    void Update()  
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        IsGrounded();
        OnWall();
        WallSlide();
        Jump();
        Flip();

        //Set animator parameters
        animator.SetBool("isRunning", horizontalInput != 0);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("onWall", OnWall());
        animator.SetFloat("velocityX", body.linearVelocityX);
        animator.SetFloat("velocityY", body.linearVelocityY);
        
    }

    //Apply inputs to our character
    void FixedUpdate()
    {
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocityY);
    }

    private void Flip()
    {
         if (horizontalInput > 0 && transform.localScale.x > 0 ||
            horizontalInput < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, jumpForce);
            animator.SetTrigger("jump");
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .2f, groundLayer);
    }

    private bool OnWall()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.right * horizontalInput, .1f, wallLayer);
    }


    private void WallSlide()
    {
        float slow = 1;
        if (OnWall() && body.linearVelocityY < 0) slow = verticalInput < 0 ? 1 : wallSlidingSpeed;

        body.linearVelocity = new Vector2(body.linearVelocityX, body.linearVelocityY * slow);
    }

}
