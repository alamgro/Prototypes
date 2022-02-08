using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private Toggle UI_ToggleInvertController;
    [Space]
    [SerializeField] private GameObject gameOverMenuPanel;
    [SerializeField] private TextMeshProUGUI UI_HighScoreLabel;
    [SerializeField] private TextMeshProUGUI UI_HighScoreCounter;
    [SerializeField] private TextMeshProUGUI UI_FinalScoreCounter;
    [Header("Other attributes")]
    [SerializeField] private Transform highscoreTextInWorldSpace;

    private int currentDistanceTraveled = 0;
    private int farthestDistanceTraveled = 0;
    private int highScore;
    private bool invertedController;

    void Awake()
    {
        _instance = this;

        #region GET PLAYERPREFS SAVED DATA
        highScore = PlayerPrefs.GetInt("Highscore", 0);
        invertedController = UI_ToggleInvertController.isOn = PlayerPrefs.GetInt("InvertedController", 1) == 1;
        #endregion

        //Set value of the wolrd space highscore counter, only if it is different to 0
        highscoreTextInWorldSpace.gameObject.SetActive(false);
        if (highScore != 0)
        {
            highscoreTextInWorldSpace.gameObject.SetActive(true);
            highscoreTextInWorldSpace.position = Vector2.up * highScore; //make Y position equals to the highscore
            highscoreTextInWorldSpace.GetComponentInChildren<TextMeshProUGUI>().SetText("Highscore: " + highScore.ToString() + "m");
        }
            

        //Get cam horizontal extents
        camHorizontalExtents = Camera.main.orthographicSize * Screen.width / Screen.height;

        UI_HighScoreLabel.SetText("HIGH SCORE");

        gameOverMenuPanel.SetActive(false);
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

    //Clear player prefs data
    public void DeleteSavedData()
    {
        highScore = 0;
        PlayerPrefs.DeleteAll();
    }

    public void ChangeControllerMode()
    {
        invertedController = UI_ToggleInvertController.isOn;
        PlayerPrefs.SetInt("InvertedController", invertedController == true ? 1 : 0);
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
        get 
        {
            if (currentDistanceTraveled >= 0)
                return currentDistanceTraveled;
            
            return 0;
        }
        set { currentDistanceTraveled = value; } 
    }

    public float CurrentPlatformLifetime
    {
        get { return currentPlatformLifetime; }
        set { currentPlatformLifetime = value; }
    }

    public float CurrentMinSpawnHeight { get { return currentMinSpawnHeight; } }

    public bool InvertedController { get { return invertedController; } }
}
