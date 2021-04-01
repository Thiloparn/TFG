using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Transform exitPoint;

    void OnTriggerEnter2D(Collider2D theObject)
    {
        if (theObject.tag == "Player")
        {
            Player.sharedInstance.animator.SetBool("isHitted", true);
            Player.sharedInstance.obstacleHitted = this;
        }
    }
}
