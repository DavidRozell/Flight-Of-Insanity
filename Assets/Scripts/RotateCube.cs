using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateCube : MonoBehaviour
{
    private bool isRotating = false;

    private float rotationSpeed = 100f;
    private bool gyroEnabled;
    private Gyroscope gyro;
    private Quaternion rot;
    public Propeller propellerScript;
    public Camera mainCamera;
    public Transform target; // the object that the camera should follow
    public Vector3 offset; // the offset distance between the camera and the target
    public bool useTargetRotation; // whether or not to use the target's rotation for the camera
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        // Check if gyroscope is available
        gyroEnabled = SystemInfo.supportsGyroscope;

        // Enable gyroscope
        if (gyroEnabled)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
    }

    void Update()
    {

        transform.Translate(Vector3.forward * 5f * Time.deltaTime);

        if (gyroEnabled)
        {
            rot = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
            transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, rot.eulerAngles.z);
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !useTargetRotation) // check if left mouse button is pressed
            {
                isRotating = true; // start rotating the cube
            }

            if (Input.GetMouseButtonUp(0) && !useTargetRotation) // check if left mouse button is released
            {
                isRotating = false; // stop rotating the cube
            }

            if (isRotating) // check if the cube should be rotating
            {
                Vector2 touchPosition = Input.mousePosition;
                if (touchPosition.x < Screen.width / 2)
                {
                    transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // rotate the cube
                }
                else
                {
                    transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime); // rotate the cube
                }
            }
        }
    }

    void LateUpdate()
    {
        // Set the position of the camera to be the target's position plus the offset
        //mainCamera.transform.position = target.position + offset;

        // If useTargetRotation is true, set the rotation of the camera to be the same as the target's rotation
        if (useTargetRotation)
        {
            // Calculate the direction from the camera to the target
            Vector3 direction = target.position - mainCamera.transform.position;

            // Calculate the rotation that the camera should have to look at the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly interpolate the camera's rotation towards the target rotation
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Collided with a wall!");
            propellerScript.enabled = false;
            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            useTargetRotation = true;
            // Loop through all child objects (excluding the parent itself)
            for (int i = 1; i < children.Length; i++)
            {
                if (children[i].name != "Main Camera")
                {
                    // Add a Rigidbody component to the current child object
                    Rigidbody childRigidbody = children[i].gameObject.AddComponent<Rigidbody>();
                }

                // Set the parent of the current child object to be the root of the game object
                children[i].gameObject.transform.parent = null;

                // Do any additional setup for the Rigidbody component as desired
                // childRigidbody.mass = 1f;
                // childRigidbody.drag = 0.5f;
                // etc.
            }

            StartCoroutine(RestartLevel());
        }
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(8f); // wait for the specified time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
