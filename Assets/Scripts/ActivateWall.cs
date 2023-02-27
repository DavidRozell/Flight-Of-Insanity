using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWall : MonoBehaviour
{
    public float zRotationRange = 360f;
    public GameObject objectMaterial;

    private void Start()
    {
        Renderer renderer = objectMaterial.GetComponent<Renderer>();
        float zRotation = Random.Range(-zRotationRange, zRotationRange);
        transform.rotation *= Quaternion.Euler(0f, 0f, zRotation);
        Color newColor = new Color(Random.value, Random.value, Random.value);
        renderer.material.color = newColor;
    }
}
