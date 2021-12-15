using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Component references
    private Rigidbody2D playerRb;
    private Animator anim;

    public ParticleSystem deathParticleSystem;

    //Other references
    GameManager gameManager;

    //Members
    private float horizontal;

    public float speed;
    public LayerMask groundLayer;
    public bool jump;
    public float jumpForce;
    public bool facingRight;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        facingRight = true;
        deathParticleSystem.Stop();
    }

    void Update()
    {
        if (transform.position.y < -6.0f)
            Death();

        horizontal = Input.GetAxisRaw("Horizontal");

        //Sets walking animation based on horizontal input
        if (horizontal != 0.0f)
            anim.SetBool("IsMoving", true);
        else
            anim.SetBool("IsMoving", false);

        //Helps set player's animation back to idle or moving after landing
        anim.SetBool("IsGrounded", IsGrounded());

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            jump = true;
            anim.SetTrigger("Jump");
        }

        //Flips when hitting 'right' and facing left
        if (horizontal > 0 && !facingRight)
            Flip();
        //Flips when hitting 'left' and facing right
        else if (horizontal < 0 && facingRight)
            Flip();
    }

    private void FixedUpdate()
    { 
        playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
        
        if(jump)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //So he doesn't fly in the air
            jump = false;
        }
        
    }

    public bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if(hit.collider != null)
        {
            return true;
        }

        return false;
    }

    //Changes rotation of the player
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coin"))
        {
            //Debug.Log("Coin picked up");
            gameManager.UpdateScore(100);
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.name == "Chest")
        {
            OpenChest openChestScript = collision.gameObject.GetComponent<OpenChest>();
            openChestScript.Open();
            gameManager.UpdateScore(500);
        }

        if(collision.gameObject.CompareTag("Level End"))
        {
            Debug.Log("Reached end of level");
        }

        if(collision.gameObject.CompareTag("Hazard"))
        {
            //Death
            Debug.Log("Collided with Hazard");
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            //Death
            //Debug.Log("Collided with Hazard");
            Death();
        }
    }

    private void Death()
    {
        gameManager.GameOver();
        deathParticleSystem.Play();
        gameObject.SetActive(false);
    }
}
