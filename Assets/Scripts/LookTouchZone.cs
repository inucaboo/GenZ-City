using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTouchZone : MonoBehaviour
{
    public Transform target;
    public float speed = 5.0f;

    private Vector3 point;

    void Start()
    {
        point = target.position;
        transform.LookAt(point);
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            // Rotate the target around the world y axis based on the delta of the touch
            point += new Vector3(-touchDeltaPosition.x * speed * Time.deltaTime, 0, 0);
        }

        transform.LookAt(point);
    }
}
