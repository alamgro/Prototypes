using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class P3_GameManager : MonoBehaviour
{
    private static P3_GameManager _instance;
    public static P3_GameManager Instance { get { return _instance; } }

    [HideInInspector] public float camHorizontalExtents; //cam half width
    [HideInInspector] public P3_PlatformSpawner platformSpawner;

    [Header("Fall area attributes")]
    [SerializeField] private GameObject fallArea;
    [SerializeField] private float heightOffset;
    [Header("Progression/dificulty attributes")]
    [SerializeField] private int unitsToProgress; //Amount of units to add dificulty progress
    [Space]
    [SerializeField] private float currentMinSpawnHeight;
    [SerializeField] private float limitMinSpawnHeight;
    [SerializeField] private float heightToIncrease;
    [Space]
    [SerializeField] private float currentPlatformLifetime;
    [SerializeField] private float limitPlatformLifetime;
    [SerializeField] private float lifetimeToDecrease;
    [Header("Distance traveled attributes")]
    [SerializeField] private TextMeshProUGUI UI_distanceTraveled;
    [Header("Menu attributes")]
    [SerializeField] private GameObject pauseMenuPanel;
    [Space]
    [SerializeField] private GameObject gameOverMenuPanel;
    [SerializeField] private TextMeshProUGUI UI_HighScoreLabel;
    [SerializeField] private TextMeshProUGUI UI_HighScoreCounter;
    [SerializeField] private TextMeshProUGUI UI_FinalScoreCounter;


    private int currentDistanceTraveled = 0;
    private int farthestDistanceTraveled = 0;
    private int highScore;

    void Awake()
    {
        _instance = this;

        //Get Saved data
        highScore = PlayerPrefs.GetInt("Highscore", 0);

        //Get cam horizontal extents
        camHorizontalExtents = Camera.main.orthographicSize * Screen.width / Screen.height;

        UI_HighScoreLabel.SetText("HIGH SCORE");

        pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        CheckDistanceTraveled();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("P3_Game");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverMenuPanel.SetActive(true);
        CheckHighscore(); //Check if there is a new highscore
        UI_HighScoreCounter.SetText(highScore.ToString() + "m");
        UI_FinalScoreCounter.SetText(farthestDistanceTraveled.ToString() + "m");
    }

    public void RelocateFallCollider(Vector3 newPosition)
    {
        //Move the area where the game detects that the player fell off
        newPosition.y -= heightOffset;
        fallArea.transform.position = newPosition * Vector2.up;
    }

    private void CheckProgress()
    {
        //Add dificulty every 100 units traveled
        if(CurrentDistanceTraveled % unitsToProgress == 0)
        {
            //The min height increases
            if(currentMinSpawnHeight < limitMinSpawnHeight)
                currentMinSpawnHeight += heightToIncrease;
            //The platform lifetime decreases
            if(currentPlatformLifetime > limitPlatformLifetime)
                currentPlatformLifetime -= lifetimeToDecrease;
        }
    }

    private void CheckDistanceTraveled()
    {
        //Update to the new farthest distance traveled if possible
        if (CurrentDistanceTraveled > farthestDistanceTraveled)
        {
            farthestDistanceTraveled = CurrentDistanceTraveled;
            CheckProgress();
            UI_distanceTraveled.text = farthestDistanceTraveled.ToString() + "m";
        }
    }

    private void CheckHighscore()
    {
        if (farthestDistanceTraveled < highScore)
            return;
        
        //Show and save new highscore
        highScore = farthestDistanceTraveled;
        PlayerPrefs.SetInt("Highscore", highScore);
        UI_HighScoreLabel.SetText("NEW\nHIGH SCORE!");

    }

    public int CurrentDistanceTraveled {
        get { return currentDistanceTraveled; }
        set { currentDistanceTraveled = value; } 
    }

    public float CurrentMinSpawnHeight { get { return currentMinSpawnHeight; } }
}
