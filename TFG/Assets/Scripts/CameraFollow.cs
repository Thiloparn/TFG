using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{

    public static CameraFollow sharedInstance;
    private float smoothTime;
    private bool following = false;
    private Vector3 velocity = Vector3.zero;
    public Transform target;


    void Awake() 
    {
        sharedInstance = this;
        Application.targetFrameRate = 60;
    }


    void Update () 
    {
        if (Mathf.Abs(Player.sharedInstance.transform.position.x - transform.position.x) >= 1)
        {
            float velocity;
            if (Player.sharedInstance.isUsingShortcut)
            {
                velocity = 864f;
                smoothTime = 0f;
            }
            else
            {
                velocity = Player.sharedInstance.rigidBody.velocity.x;
                smoothTime = 0.5f;
            }

            if (Mathf.Abs(velocity) < 0.01)
            {
                following = false;
            }

            if (Vector2.Distance(Player.sharedInstance.idlePosition, Player.sharedInstance.transform.position) >= 128 || following)
            {
                following = true;

                if (velocity > 0)
                {
                    follow(new Vector2(0.1f, 0.12f), target.position);
                }
                else
                {
                    follow(new Vector2(0.9f, 0.12f), target.position);
                }
            }
            else
            {
                follow(new Vector2(0.5f, 0.12f), Player.sharedInstance.idlePosition);
            }
        }
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
