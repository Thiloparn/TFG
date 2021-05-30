using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Transform exitPoint;

    void OnTriggerEnter2D(Collider2D theObject)
    {
        if (theObject.tag == "Player")
        {
            float positionX = Player.sharedInstance.transform.position.x - transform.position.x > 0 ? exitPoint.position.x + 32 : transform.position.x - 32;
            Player.sharedInstance.transform.position = new Vector3(positionX, Player.sharedInstance.transform.position.y, Player.sharedInstance.transform.position.z);
        }
    }

    void OnTriggerStay2D(Collider2D theObject)
    {
        if (theObject.tag == "Player")
        {
            float positionX = Player.sharedInstance.transform.position.x - transform.position.x > 0 ? exitPoint.position.x + 32 : transform.position.x - 32;
            Player.sharedInstance.transform.position = new Vector3(positionX, Player.sharedInstance.transform.position.y, Player.sharedInstance.transform.position.z);
        }
    }
}
