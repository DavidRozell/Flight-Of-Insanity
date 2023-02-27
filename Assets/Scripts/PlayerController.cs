using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool isRotating = false;

    private float rotationSpeed = 100f;
    private bool gyroEnabled;
    private Gyroscope gyro;
    private Quaternion rot;
    public Propeller propellerScript;
    public Camera mainCamera;
    public Transform target;
    public Vector3 offset;
    public bool useTargetRotation;
    private Quaternion initialRotation;
    private AudioSource musicSource;
    public AudioSource newRecordSource;
    public AudioSource audioSource;
    public AudioClip explodeAudioClip;
    public AudioClip musicClip;
    public Button restartGame;
    public bool destroyed;
    public float speedMultiplier = 5f;


    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.Play();
        audioSource.volume = 0.6f;
        initialRotation = transform.rotation;
        gyroEnabled = SystemInfo.supportsGyroscope;

        if (gyroEnabled)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
    }

    void Update()
    {

        transform.Translate(Vector3.forward * speedMultiplier * Time.deltaTime);

        if (gyroEnabled)
        {
            rot = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
            transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, rot.eulerAngles.z);
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !useTargetRotation)
            {
                isRotating = true;
            }

            if (Input.GetMouseButtonUp(0) && !useTargetRotation)
            {
                isRotating = false;
            }

            if (isRotating)
            {
                Vector2 touchPosition = Input.mousePosition;
                if (touchPosition.x < Screen.width / 2)
                {
                    transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (useTargetRotation)
        {
            Vector3 direction = target.position - mainCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            audioSource.volume = 1f;
            audioSource.clip = explodeAudioClip;
            audioSource.Play();
            audioSource.loop = false;
            Debug.Log("Collided with a wall!");
            propellerScript.enabled = false;
            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            useTargetRotation = true;
            for (int i = 1; i < children.Length; i++)
            {
                if (children[i].name != "Main Camera")
                {
                    Rigidbody childRigidbody = children[i].gameObject.AddComponent<Rigidbody>();
                }
                children[i].gameObject.transform.parent = null;
            }

            restartGame.gameObject.SetActive(true);
        }
    }
}
