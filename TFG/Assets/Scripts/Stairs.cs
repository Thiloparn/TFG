using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public Transform exitPoint;
    public MeshRenderer meshRenderer;
    public TextMesh text;
    public BoxCollider2D boxCollider;

    public int goToLevel;
    private bool isUsable = true;

    private void OnTriggerEnter2D(Collider2D theObject)
    {
        if(theObject.tag == "Obstacle")
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
        if(theObject.tag == "Obstacle")
        {
            isUsable = false;
        }

        if (theObject.tag == "Player" && Input.GetKeyDown(KeyCode.Mouse1) && isUsable && Player.sharedInstance.animator.GetBool("IsHitted"))
        {
            LevelGenerator.sharedInstance.level = goToLevel;
            LevelGenerator.sharedInstance.changeLevel = true;
        }
    }


    private void Awake()
    {
        meshRenderer.sortingLayerName = "Shortcuts text";

        goToLevel = Mathf.FloorToInt(Random.Range(0, 5));
        while(goToLevel == LevelGenerator.sharedInstance.level)
        {
            goToLevel = Mathf.FloorToInt(Random.Range(0, 5));
        }

        text.text = goToLevel.ToString();
    }
}
