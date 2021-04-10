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
    public SpriteRenderer spriteRenderer;

    public float speed = 7.0f, jumpForce = 17.0f, hitForce = 10f, distanceHitting = 5;
    public int slidingTime = 70;
    private float timerHit = 1.0f, timerSlide = 1.0f, timerInvincible = 0f;
    public Vector2 idlePosition = new Vector2(0, -2);
    private bool isFacingRight = true;
    public bool isInvincible = false, isUsingShortcut = false;


    void Awake()
    {
        sharedInstance = this;
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        rigidBody.position = idlePosition;
        rigidBody.velocity = new Vector2(0, 0);
    }


    void Update()
    {
        if (isUsingShortcut)
        {
            spriteRenderer.enabled = false;
            rigidBody.velocity = new Vector2(0, 0);
        }
        else
        {
            spriteRenderer.enabled = true;

            invincible();

            isHitting();

            if (animator.GetBool("isHitted"))
            {
                beingHitted();
            }
            else
            {
                timerHit = 1.0f;
                animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

                if (Input.GetKey(KeyCode.LeftShift) && !animator.GetBool("IsJumping"))
                {
                    animator.SetBool("IsSliding", true);
                    timerSlide -= Time.deltaTime / slidingTime;
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x * timerSlide, rigidBody.velocity.y);
                }
                else
                {
                    animator.SetBool("IsSliding", false);
                    timerSlide = 1.0f;

                    if (Input.GetKey(KeyCode.D))
                    {
                        moveRight();
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        moveLeft();
                    }
                    else
                    {
                        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                        idlePosition = rigidBody.position;
                    }
                }

                jump();
                
                animator.SetBool("IsJumping", !IsOnTheFloor());
            }
        }
    }


    void invincible()
    {
        if (isInvincible)
        {
            timerInvincible += Time.deltaTime;
            if (timerInvincible > 2f)
            {
                isInvincible = false;
                timerInvincible = 0f;
            }
        }
    }


    void isHitting()
    {
        if (obstacleHitted != null)
        {
            float distance = Vector2.Distance(obstacleHitted.transform.position, this.transform.position);
            float maxDistance;

            if (isFacingRight)
            {
                maxDistance = distanceHitting;
            }
            else
            {
                float obstacleSize = Vector2.Distance(obstacleHitted.transform.position, obstacleHitted.exitPoint.transform.position);
                maxDistance = distanceHitting + obstacleSize;
            }

            if (distance < maxDistance || !IsOnTheFloor())
            {
                animator.SetBool("isHitted", true);
            }
            else
            {
                animator.SetBool("isHitted", false);
                obstacleHitted = null;
                rigidBody.velocity = new Vector2(0f, 0f);
            }
        }
    }


    void beingHitted()
    {
        animator.SetBool("IsSliding", false);
        float orientedHitForce = hitForce;

        if (isFacingRight && obstacleHitted.exitPoint.transform.position.x >= this.transform.position.x)
        {
            orientedHitForce = -hitForce;
        }

        timerHit += Time.deltaTime * 10;
        rigidBody.velocity = new Vector2(orientedHitForce, -rigidBody.gravityScale * timerHit);
    }


    void moveRight()
    {
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        if (!isFacingRight)
        {
            isFacingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }


    void moveLeft()
    {
        rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);

        if (isFacingRight)
        {
            isFacingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }


    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("IsJumping"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
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
}
