using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public Transform target;
    public float duration = 1f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private float timeElapsed = 0f;
    private CameraTransition cameraTransition;

    private void Start()
    {
        cameraTransition = gameObject.GetComponent<CameraTransition>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        targetPosition = target.position;
        targetRotation = target.rotation;
    }

    private void Update()
    {
        if (transform.position != targetPosition && transform.rotation != targetRotation)
        {
            timeElapsed += Time.deltaTime;

            float t = Mathf.Clamp01(timeElapsed / duration);
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
        }
        else
        {
            cameraTransition.enabled = false;
        }
    }
}
