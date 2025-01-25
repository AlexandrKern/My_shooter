using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0, 5, -10);

    [Header("Follow Smoothness")]
    public float smoothSpeed = 0.125f;

    private bool isShaking = false;

    void LateUpdate()
    {
        if (target == null || isShaking)
            return; 

        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        transform.LookAt(target);
    }

    public void SetShaking(bool shaking)
    {
        isShaking = shaking;
    }
}



