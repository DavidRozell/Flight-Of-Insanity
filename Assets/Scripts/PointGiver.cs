using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointGiver : MonoBehaviour
{
    public Text scoreText;
    private GameManager gameManager;
    private PlayerController playerController;
    private AudioSource audioSource;
    private bool hit;

    void Start()
    {
        playerController = GameObject.Find("Plane").GetComponent<PlayerController>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Plane") && !hit && !playerController.destroyed)
        {
            hit = true;
            audioSource.Play();
            gameManager.score++;
            scoreText.text = "Score: " + gameManager.score.ToString();

            if (gameManager.score == gameManager.nextSpeedUp)
            {
                gameManager.nextSpeedUp += 10;
                playerController.speedMultiplier += 3;
               StartCoroutine(gameManager.SpeedUpFlash());
            }    

        }
    }
}
