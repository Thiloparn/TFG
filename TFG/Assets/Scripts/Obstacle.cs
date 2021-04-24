using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private Rigidbody2D rigidBody;
    public BoxCollider2D boxCollider;
    public Transform exitPoint;

    public bool isStatic, isBreakable;
    public float speed;
    public List<string> zone = new List<string>();


    void OnTriggerEnter2D(Collider2D theObject)
    {
        if (theObject.tag == "Player" && !Player.sharedInstance.isInvincible && !Player.sharedInstance.isUsingShortcut)
        { 
            Player.sharedInstance.animator.SetBool("IsHitted", true);
            Player.sharedInstance.obstacleHitted = this;
            Player.sharedInstance.isInvincible = true;
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
    }


    public void Initialize(){
        Random.InitState((int) System.DateTime.Now.Ticks);
        float random = Random.Range(0, 2);
        if(random==0){
            this.speed = -this.speed;
        }
    }
}
