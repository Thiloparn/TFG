﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Player : MonoBehaviour
{

    public static Player sharedInstance;
    public Rigidbody2D rigidBody;
    public Animator animator;
    public LayerMask groundLayerMask;
    public SpriteRenderer spriteRenderer;
    public Transform attackPoint;
    public PlayerInput playerInput;

    public List<float> speeds = new List<float>();
    public float actualSpeed, jumpForce, hitForce, distanceHitting;
    public int slidingTime;
    private float timerHit = 1.0f, timerSlide = 1.0f, timerInvincible = 0f;
    public Vector2 idlePosition;
    private bool isFacingRight = true;
    public bool isInvincible, isUsingShortcut;
    public float attackRange;

    public List<Vector2> obstacleHittedInfo = new List<Vector2>();

    void Awake()
    {
        sharedInstance = this;
    }


    void Start()
    {
        string rebinds = loadData();
        if (string.IsNullOrEmpty(rebinds)) { return; }
        playerInput.actions.LoadBindingOverridesFromJson(rebinds);

        rigidBody.position = idlePosition;
        rigidBody.velocity = new Vector2(0, 0);
        actualSpeed = speeds[0];
        slidingTime = 5;
    }


    private void Update()
    {
        if (!PauseMenu.sharedInstance.isActive)
        {
            if (transform.position.y <= -97f)
            {
                transform.position = new Vector3(transform.position.x, -96f, transform.position.z);
            }
            else
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

                    if (animator.GetBool("IsHitted"))
                    {
                        beingHitted();
                    }
                    else
                    {
                        timerHit = 1.0f;
                        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

                        if (playerInput.actions.FindAction("Slide").IsPressed() && !animator.GetBool("IsJumping") && !ItemsUI.sharedInstance.isActive)
                        {
                            animator.SetBool("IsSliding", true);
                            timerSlide -= Time.deltaTime / slidingTime;
                            timerSlide = timerSlide <= 0 ? 0 : timerSlide;
                            float direction = isFacingRight ? 1f : -1f;
                            float slidingVelocity = direction * actualSpeed * timerSlide;
                            rigidBody.velocity = new Vector2(slidingVelocity, rigidBody.velocity.y);
                        }
                        else
                        {
                            animator.SetBool("IsSliding", false);
                            timerSlide = 1.0f;

                            if (ItemsUI.sharedInstance.isActive)
                            {
                                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                                animator.SetFloat("Speed", rigidBody.velocity.x);
                            }
                            else
                            {
                                if (playerInput.actions.FindAction("Right").IsPressed())
                                {
                                    moveRight();
                                }
                                else if (playerInput.actions.FindAction("Left").IsPressed())
                                {
                                    moveLeft();
                                }
                                else
                                {
                                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                                }
                            }
                        }

                        jump();

                        animator.SetBool("IsJumping", !IsOnTheFloor());

                        if (Mathf.Abs(rigidBody.velocity.x) < 0.01)
                        {
                            idlePosition = rigidBody.position;
                        }

                        if (rigidBody.velocity.y < 0)
                        {
                            float yVelocity = transform.position.y + rigidBody.velocity.y * Time.deltaTime <= -96f ? 0f : rigidBody.velocity.y;
                            rigidBody.velocity = new Vector2(rigidBody.velocity.x, yVelocity);
                        }
                    }
                }
            }
        }
    }


    void invincible()
    {
        if (isInvincible)
        {
            InvokeRepeating("flash", 0f, 0.1f);
            timerInvincible += Time.deltaTime;
            if (timerInvincible > 2f)
            {
                CancelInvoke();
                isInvincible = false;
                timerInvincible = 0f;
            }
        }
    }

    void flash()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }


    void isHitting()
    {
        if (obstacleHittedInfo.Count > 0)
        {
            float distance = Vector2.Distance(obstacleHittedInfo[0], this.transform.position);
            float maxDistance;

            if (isFacingRight)
            {
                maxDistance = distanceHitting;
            }
            else
            {
                float obstacleSize = Vector2.Distance(obstacleHittedInfo[0], obstacleHittedInfo[1]);
                maxDistance = distanceHitting + obstacleSize;
            }

            if (distance < maxDistance || !IsOnTheFloor())
            {
                animator.SetBool("IsHitted", true);
            }
            else
            {
                animator.SetBool("IsHitted", false);
                obstacleHittedInfo.Clear();
                rigidBody.velocity = new Vector2(0f, 0f);
            }
        }
    }


    void beingHitted()
    {
        animator.SetBool("IsSliding", false);
        float orientedHitForce = isFacingRight ? -hitForce : hitForce;

        timerHit += Time.deltaTime * 10;

        float verticalForce;
        if (IsOnTheFloor())
        {
            verticalForce = 0;
        }
        else
        {
            verticalForce = -rigidBody.gravityScale * timerHit;
        }

        rigidBody.velocity = new Vector2(orientedHitForce, verticalForce);
    }


    void moveRight()
    {
        rigidBody.velocity = new Vector2(actualSpeed, rigidBody.velocity.y);

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
        rigidBody.velocity = new Vector2(-actualSpeed, rigidBody.velocity.y);

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
        if (playerInput.actions.FindAction("Jump").IsPressed() && !animator.GetBool("IsJumping") && !ItemsUI.sharedInstance.isActive)
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

    public string loadData()
    {
        string res = "";
        string path = Application.persistentDataPath + "/controls.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            res = (string)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }

        return res;
    }

}
