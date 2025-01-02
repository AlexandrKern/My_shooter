using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Объект, за которым следует камера

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0, 5, -10); // Смещение камеры относительно цели

    [Header("Follow Smoothness")]
    public float smoothSpeed = 0.125f; // Скорость сглаживания движения

    void LateUpdate()
    {
        if (target == null)
            return;

        // Вычисляем желаемую позицию камеры
        Vector3 desiredPosition = target.position + offset;

        // Плавное перемещение камеры к желаемой позиции
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        // Поворачиваем камеру, чтобы она смотрела на цель
        transform.LookAt(target);
    }
}
