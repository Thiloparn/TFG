using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float smoothTime = 0.4f;
    private bool following = false;
    private Vector3 velocity = Vector3.zero;
    public Transform target;


    void Awake() 
    {
        Application.targetFrameRate = 60;
    }

    void Update () 
    {
        float velocity = Player.sharedInstance.rigidBody.velocity.x;

        if (Mathf.Abs(velocity) < 0.01)
        {
            following = false;
        }

        if (!freeRoaming() || following)
        {
            following = true;

            if (velocity > 0)
            {
                follow(new Vector2(0.1f, 0.12f), target.position);
            } else
            {
                follow(new Vector2(0.9f, 0.12f), target.position);
            }
        } else {
            follow(new Vector2(0.5f, 0.12f), Player.sharedInstance.idlePosition);
        }
    }

    bool freeRoaming()
    {
        float num1 = Player.sharedInstance.idlePosition.x;
        float num2 = Player.sharedInstance.rigidBody.position.x;
        float distance = 0;

        if(num1 >= num2)
        {
            distance = num1 - num2;
        } else
        {
            distance = num2 - num1;
        }

        return distance < 7;
    }

    void follow(Vector2 offset, Vector2 targetPosition)
    {
        Vector3 tP3 = new Vector3(targetPosition.x, targetPosition.y, target.position.z);
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(tP3);
        Vector3 delta = tP3 - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(offset.x, offset.y, point.z));
        Vector3 destination = transform.position + delta;

        destination = new Vector3(destination.x, offset.y, destination.z);

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);
    }
}
