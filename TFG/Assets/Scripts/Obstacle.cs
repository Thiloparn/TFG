using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public Transform exitPoint;

    public bool isStatic, isBreakable;
    public float speed;
    public List<string> zone = new List<string>(); 


    void OnTriggerEnter2D(Collider2D theObject)
    {
        if (theObject.tag == "Player")
        {
            Player.sharedInstance.animator.SetBool("isHitted", true);
            Player.sharedInstance.obstacleHitted = this;
        }

        if(theObject.tag == "Obstacle")
        {
            this.speed = -this.speed;
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
        float random = Random.Range(0, 10);
        if(random<5){
            this.speed = -this.speed;
        }
    }
}
