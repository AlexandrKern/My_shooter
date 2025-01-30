using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float duration = 0.18f;
    [SerializeField] private float magnitude = 0.3f;

    private CameraFollow cameraFollow;

    private void Awake()
    {
        cameraFollow = GetComponent<CameraFollow>();
    }

    public IEnumerator Shake()
    {
        if (cameraFollow != null)
            cameraFollow.SetShaking(true);

        Vector3 originalPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        if (cameraFollow != null)
            cameraFollow.SetShaking(false);
    }

}



