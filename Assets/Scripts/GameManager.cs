using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject prefab;
    public float distanceIncrement;
    public float spawnDelay;
    public PlayerController playerController;
    public CameraTransition cameraTransition;
    public Button startButton;
    public Text scoreText;
    public int score;
    public Text highScoreText;
    public int highScore;
    public Text newHighScoreText;
    public Text speedUpText;
    public AudioSource gameAudio;
    public AudioClip newRecordClip;
    public AudioClip speedUpClip;
    public int nextSpeedUp;
    public int spawnedPrefabs = 0;
    private float distance;
    private bool newHighScoreActivated;
    private bool gameStarted;
    private float lastSpawnTime = 0f;

    private void Start()
    {
        startButton.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        highScore = PlayerPrefs.GetInt("High Score");
        highScoreText.text = "High Score: " + highScore.ToString();
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime >= spawnDelay && gameStarted && !playerController.destroyed)
        {
            SpawnPrefab();
        }

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("High Score", highScore);
            PlayerPrefs.Save();
            if (!newHighScoreActivated)
            {
                gameAudio.clip = newRecordClip;
                gameAudio.Play();
                newHighScoreText.gameObject.SetActive(true);
                StartCoroutine(NewHighScoreFlash());
                newHighScoreActivated = true;
            }
        }
    }

    public void StartGame()
    {
        highScoreText.gameObject.SetActive(false);
        StartCoroutine(EnablePlayer());
        cameraTransition.enabled = true;
        startButton.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SpawnPrefab()
    {
        if (!playerController.destroyed)
        {
            distance += distanceIncrement;
            Vector3 spawnPosition = prefab.transform.position;
            spawnPosition += new Vector3(0f, 0f, distance);
            Instantiate(prefab, spawnPosition, Quaternion.identity);
            spawnedPrefabs++;
            lastSpawnTime = Time.time;
        }
    }

    IEnumerator EnablePlayer()
    {
        yield return new WaitForSeconds(1f);
        scoreText.gameObject.SetActive(true);
        playerController.enabled = true;
        prefab.SetActive(true);
        gameStarted = true;
    }

    IEnumerator NewHighScoreFlash()
    {

        for (int i = 0; i < 10; i++)
        {
            newHighScoreText.enabled = true;
            yield return new WaitForSeconds(0.15f);
            newHighScoreText.enabled = false;
            yield return new WaitForSeconds(0.15f);
        }
        newHighScoreText.gameObject.SetActive(false);
    }

    public IEnumerator SpeedUpFlash()
    {
        speedUpText.gameObject.SetActive(true);
        gameAudio.clip = speedUpClip;
        gameAudio.Play();
        for (int i = 0; i < 10; i++)
        {
            speedUpText.enabled = true;
            yield return new WaitForSeconds(0.15f);
            speedUpText.enabled = false;
            yield return new WaitForSeconds(0.15f);
        }
        speedUpText.gameObject.SetActive(false);
    }
}
