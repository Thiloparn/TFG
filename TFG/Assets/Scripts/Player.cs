using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player sharedInstance;
    public Rigidbody2D rigidBody;
    public Animator animator;
    public LayerMask groundLayerMask;
    public Obstacle obstacleHitted = null;

    public float speed = 7.0f;
    public float jumpForce = 17.0f;
    public float hitForce = 10f;
    public float distanceHitting = 5f;
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
        if(obstacleHitted != null)
        {
            isHitting();
        }

        if (animator.GetBool("isHitted"))
        {
            float orientedHitForce = hitForce;

            if (facingRight && obstacleHitted.exitPoint.transform.position.x >= this.transform.position.x)
            {
                orientedHitForce = -hitForce;
            }


            rigidBody.velocity = new Vector2(orientedHitForce, -rigidBody.gravityScale); ;

        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

            if (Input.GetKey(KeyCode.LeftShift) && !animator.GetBool("IsJumping"))
            {
                animator.SetBool("IsSliding", true);
            }
            else
            {
                animator.SetBool("IsSliding", false);

                if (Input.GetKey(KeyCode.D))
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
                else if (Input.GetKey(KeyCode.A))
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

    void isHitting()
    {
        float distance = Vector2.Distance(obstacleHitted.transform.position, this.transform.position);
        float maxDistance;

        if(facingRight)
        {
            maxDistance = distanceHitting;
        }
        else
        {
            float obstacleSize = Vector2.Distance(obstacleHitted.transform.position, obstacleHitted.exitPoint.transform.position);
            maxDistance = distanceHitting + obstacleSize;
        }

        if(distance < maxDistance)
        {
            animator.SetBool("isHitted", true);
        } else
        {
            animator.SetBool("isHitted", false);
            obstacleHitted = null;
        }
    }
}
