using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public Transform exitPoint;
    public MeshRenderer meshRenderer;
    public TextMesh text;

    public int goToLevel;


    void OnTriggerStay2D(Collider2D theObject)
    {
        if (theObject.tag == "Player" && Input.GetKeyDown(KeyCode.Mouse1))
        {
            LevelGenerator.sharedInstance.level = goToLevel;
            LevelGenerator.sharedInstance.changeLevel = true;
        }
    }


    private void Awake()
    {
        meshRenderer.sortingLayerName = "UI";

        goToLevel = Mathf.FloorToInt(Random.Range(0, 5));
        while(goToLevel == LevelGenerator.sharedInstance.level)
        {
            goToLevel = Mathf.FloorToInt(Random.Range(0, 5));
        }

        text.text = goToLevel.ToString();
    }
}
