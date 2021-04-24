using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcut : MonoBehaviour
{

    public Transform exitPoint;
    public MeshRenderer meshRenderer;
    public TextMesh text;
    public Rigidbody2D rigidBody;

    public float maxtime;
    public float timer;
    public string start, end, zone;

    private bool isUsable = true;
    private bool isInUse = false, isActive = true;
    private float goTo;

    private void OnTriggerEnter2D(Collider2D theObject)
    {
        if (theObject.tag == "Obstacle")
        {
            isUsable = false;
        }
    }

    private void OnTriggerExit2D(Collider2D theObject)
    {
        isUsable = true;
    }

    void OnTriggerStay2D(Collider2D theObject)
    {
        if (theObject.tag == "Obstacle")
        {
            isUsable = false;
        }

        if (theObject.tag == "Player" && isActive && isUsable)   
        {
            if (timer == 0f && Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (LevelGenerator.sharedInstance.zone == "Metropolis")
                {
                    int random;
                    if (end == "Halfway")
                    {
                        random = Random.Range(20, 41);
                    }
                    else
                    {
                        random = Random.Range(40, 61);
                    }

                    goTo = LevelGenerator.sharedInstance.floorsSpawned[random].transform.position.x;
                    isInUse = true;
                }
                else
                {
                    if(LevelGenerator.sharedInstance.zone == "Community")
                    {
                        LevelGenerator.sharedInstance.level = 2;
                        
                    }
                    else
                    {
                        LevelGenerator.sharedInstance.level = 4;
                    }

                    LevelGenerator.sharedInstance.changeLevel = true;
                    LevelGenerator.sharedInstance.useStairOrElevator();
                }
            }
        }
    }


    private void Awake()
    {
        timer = Random.Range(5, maxtime + 1);
        meshRenderer.sortingLayerName = "Shortcuts text";
        rigidBody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if(Vector3.Distance(Player.sharedInstance.transform.position, transform.position) < 384f)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime % 60;
            } 
            else
            {
                timer = 0f;
            }

            text.text = Mathf.FloorToInt(timer).ToString();
        }

        if (isInUse)
        {
            Player.sharedInstance.isUsingShortcut = true;
            meshRenderer.enabled = false;
            if (transform.position.x < goTo)
            {
                rigidBody.velocity = new Vector2(864f, 0f);
                Player.sharedInstance.transform.position = new Vector3(transform.position.x, Player.sharedInstance.transform.position.y, Player.sharedInstance.transform.position.z);
            }
            else
            {
                rigidBody.velocity = new Vector2(0f, 0f);
                Player.sharedInstance.isUsingShortcut = false;
                Player.sharedInstance.isInvincible = true;
                isInUse = false;
                isActive = false;
            }
        }
    }
}
