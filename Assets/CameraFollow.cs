using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float distance = 0.8f;
    
    [SerializeField]
    private float height = 0.15f;
    void LateUpdate()
    {
        var targetRotationAngle = target.eulerAngles.y;
        var targetHeight = target.position.y + height;

        // Convert angle into a rotation
        var currentRotation = Quaternion.Euler(0, targetRotationAngle, 0);

        // Set the position of the camera on the x-z plane to
        // distance kilometers behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);

        // Always look at the target
        transform.LookAt(target);
    }
}
