using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private float smoothSpeed = 0.125f;
    private Vector3 velocity = Vector3.zero;
    void FixedUpdate()
    {
        //transform.position = target.position;
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothSpeed);
        transform.rotation = target.rotation;
    }
}
