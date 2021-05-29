using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public Transform exitPoint;

    public bool isStatic, isBreakable, isBroken;
    public float speed;
    public List<string> zone = new List<string>();


    void OnTriggerEnter2D(Collider2D theObject)
    {
        if (theObject.tag == "Player" && !Player.sharedInstance.isInvincible && !Player.sharedInstance.isUsingShortcut)
        { 
            Player.sharedInstance.animator.SetBool("IsHitted", true);
            Player.sharedInstance.obstacleHittedInfo.Add(this.transform.position);
            Player.sharedInstance.obstacleHittedInfo.Add(this.exitPoint.transform.position);
            Player.sharedInstance.obstacleHittedInfo.Add(this.rigidBody.velocity);
            Player.sharedInstance.isInvincible = true;
            SpeedUI.sharedInstance.obstacleHitted();
        }
    }


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        rigidBody.velocity = new Vector2(0, 0);
    }


    void Update()
    {
        if(!this.isStatic){
            this.rigidBody.velocity = new Vector2(this.speed, this.rigidBody.velocity.y);
        }

        /*if (isBroken)
        {
            LevelGenerator.sharedInstance.obstaclesSpawned.Remove(this);
            spriteRenderer.enabled = false;
            Destroy(this);
        }*/
    }


    public void Initialize(){
        Random.InitState((int) System.DateTime.Now.Ticks);
        float random = Random.Range(0, 2);
        if(random==0 && !isStatic){
            this.speed = -this.speed;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void brake()
    {
        LevelGenerator.sharedInstance.obstaclesSpawned.Remove(this);
        Destroy(this.gameObject);
    }
}
