using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject target;
    private Rigidbody2D targetRb;

    private float smoothSpeed = 0.9f;
    private Vector3 offset = new Vector3(1f, 1f, -40f);

    private bool usePrediction = false;
    private float predictionStrength = 0.05f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        targetRb = target.GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 prediction = Vector3.zero;

            // Предсказание движения на основе скорости
            if (usePrediction && targetRb != null)
            {
                prediction = targetRb.velocity * predictionStrength;
            }

            Vector3 desiredPosition = target.transform.position + offset + prediction;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
