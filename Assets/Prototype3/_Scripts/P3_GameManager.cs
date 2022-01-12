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

    private int currentDistanceTraveled = 0;
    private int farthestDistanceTraveled = 0;

    void Awake()
    {
        _instance = this;

        //Get cam horizontal extents
        camHorizontalExtents = Camera.main.orthographicSize * Screen.width / Screen.height;
    }

    private void Update()
    {
        CheckDistanceTraveled();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("P3_Game");
    }

    public void RelocateFallCollider(Vector3 newPosition)
    {
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
            UI_distanceTraveled.text = farthestDistanceTraveled.ToString();
        }
    }

    public int CurrentDistanceTraveled {
        get { return currentDistanceTraveled; }
        set { currentDistanceTraveled = value; } 
    }
}
