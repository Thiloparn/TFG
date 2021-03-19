using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player sharedInstance;
    public Rigidbody2D rigidBody;
    public Animator animator;
    public LayerMask groundLayerMask;

    public float speed = 7.0f;
    public float jumpForce = 17.0f;
    public Vector2 idlePosition = new Vector2(0, -2);
    private bool facingRight = true;
    

    void Awake()
    {
        sharedInstance = this;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidBody.position = idlePosition;
        rigidBody.velocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

        if (Input.GetKey(KeyCode.LeftControl) && !animator.GetBool("IsJumping"))
        {
            animator.SetBool("IsSliding", true);
        }
        else
        {
            animator.SetBool("IsSliding", false);

            if (Input.GetKey(KeyCode.RightArrow))
            {
                rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

                if (!facingRight)
                {
                    facingRight = true;
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);
                if (facingRight)
                {
                    facingRight = false;
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                }
            }
            else
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                idlePosition = rigidBody.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("IsJumping"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
        }

        animator.SetBool("IsJumping", !IsOnTheFloor());

        
    }

    bool IsOnTheFloor()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 2.2f, groundLayerMask.value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
