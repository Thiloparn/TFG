using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer brokenSpriteRenderer;
    public Transform exitPoint;

    public bool isStatic, isBreakable, isBroken;
    public float speed;
    public List<string> zone = new List<string>();

    public List<Floor> floors = new List<Floor>();


    void OnTriggerEnter2D(Collider2D theObject)
    {
        if (theObject.tag == "Player" && !Player.sharedInstance.isInvincible && !Player.sharedInstance.isUsingShortcut && !this.isBroken)
        { 
            Player.sharedInstance.animator.SetBool("IsHitted", true);
            Player.sharedInstance.obstacleHittedInfo.Add(this.transform.position);
            Player.sharedInstance.obstacleHittedInfo.Add(this.exitPoint.transform.position);
            Player.sharedInstance.obstacleHittedInfo.Add(this.rigidBody.velocity);
            Player.sharedInstance.isInvincible = true;
            SpeedUI.sharedInstance.obstacleHitted();
        }
        if (theObject.tag == "Wall")
        {
            this.speed = -this.speed;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

    }


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if(brokenSpriteRenderer != null)
        {
            brokenSpriteRenderer.enabled = false;
        }
    }


    void Start()
    {
        rigidBody.velocity = new Vector2(0, 0);

        floors.Clear();
        if (exitPoint.position.x < LevelGenerator.sharedInstance.floorsSpawned[LevelGenerator.sharedInstance.floorsSpawned.Count - 1].exitPoint.position.x)
        {
            floors.AddRange(LevelGenerator.sharedInstance.floorsSpawned);
        }
        else
        {
            floors.AddRange(LevelGenerator.sharedInstance.nextFloorsSpawned);
        }
    }


    void Update()
    {
        if(!this.isStatic){
            this.rigidBody.velocity = new Vector2(this.speed, this.rigidBody.velocity.y);
        }

        if (floors[0] != null)
        {
            bool leftLimit = floors[0].transform.position.x - exitPoint.position.x > 0;
            bool rightLimit = exitPoint.position.x - floors[floors.Count - 1].exitPoint.position.x > 0;

            if (leftLimit || rightLimit)
            {
                float positionX = leftLimit ? floors[0].transform.position.x + 1 : floors[floors.Count - 1].exitPoint.position.x - 1;
                transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
                this.speed = -this.speed;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
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
        this.speed = 0;
        this.isBroken = true;
        spriteRenderer.enabled = false;
        brokenSpriteRenderer.enabled = true;
        InvokeRepeating("flash", 0f, 0.1f);
        Invoke("destroy", 0.5f);
    }

    void flash()
    {
        brokenSpriteRenderer.enabled = !brokenSpriteRenderer.enabled;
    }

    void destroy()
    {
        LevelGenerator.sharedInstance.obstaclesSpawned.Remove(this);
        Destroy(this.gameObject);
    }

}
