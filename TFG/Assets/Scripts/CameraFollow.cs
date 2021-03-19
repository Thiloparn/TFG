using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float dampTime = 0.4f;
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
            follow(new Vector2(0.5f, 0.12f));
        }

        if (!freeRoaming())
        {
            //Player.sharedInstance.idlePosition.x = Player.sharedInstance.rigidBody.position.x;

            if (velocity > 0)
            {
                follow(new Vector2(0.1f, 0.12f));
            } else
            {
                follow(new Vector2(0.9f, 0.12f));
            }
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

    void follow(Vector2 offset)
    {
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(offset.x, offset.y, point.z));
        Vector3 destination = transform.position + delta;

        destination = new Vector3(destination.x, offset.y, destination.z);

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
}
